#include <iso646.h>
#include <io.h>
#include <stdint.h>
#include <string>
#include <windows.h>

#include "rtl_exec.h"

class Port {
public:
    std::string name;
#ifdef _WIN32
    HANDLE port;
#else
    int port;
#endif
    uint32_t baud;
    uint32_t stop;
    uint32_t pairty;
    uint32_t databits;
    
    // Serial connection line statuses
    bool carrier_detect;
    bool data_set_ready;
    bool data_terminal_ready;

    bool carrier_dropped;

    // Serial line controls
    uint32_t rts_flow_control;
    uint32_t dtr_flow_control;
    uint32_t xon_flow_control;

    // Serial Port Properties
    bool use_parity;
    bool dsr_sensitivity;
    bool xon_continue;
    bool discard_nulls;
    bool abort_on_error;

    char  xon_char;
    char  xoff_char;
    char  error_char;
    char  eof_char;
    char  evt_char;
    uint16_t xon_limit;
    uint16_t xoff_limit;

    // Serial line event flags to momitor
    uint32_t evt_flags;
    uint32_t serial_status;

    BYTE *buf;          // Read data buffer...
};

static Port *check_port(void *pp)
{
    Port *p = static_cast<Port *>(pp);
    if (p == NULL) {
        throw RtlException(Exception_serial$InvalidPort, "");
    }
    return p;
}

namespace rtl {

static int set_topts(const CONN *conn, int rate);

void *serial$open(const std::string name, Number baud, bool flowControl)
{
    Port *p = new Port;
    p->name.append(name);
    p->baud = number_to_uint32(baud);
    p->buf = new BYTE[16384];           // Default to a 16k buffer...

    p->carrier_detect           = false;
    p->data_set_ready           = false;
    p->data_terminal_ready      = false;
    p->carrier_dropped          = false;
    p->rts_flow_control         = false;
    p->dtr_flow_control         = false;
    p->xon_flow_control         = false;

    p->port = open(name.c_str(), O_FLAGS);
    if (p->port < 0) {
    }

    // Make sure device is a TTY.
    if (not isatty(p->port)) {
        close(p->port);
    }
    if (set_topts(conn, rate)) {
        close(p->port);
    }
    // Flush the I/O buffers.
    if (tcflush(p->port, TCIOFLUSH)) {
        log_msg(LOG_ERR, "%s:%d [%s] Error flushing TTY I/O buffers : %s\n",
                __FILE__, __LINE__, conn->name, strerror(errno));
        close(p->port);
    }
    switch (rate) {
        case B110:  conn->byte_usec = 91000; break;
        case B300:  conn->byte_usec = 34000; break;
        case B600:  conn->byte_usec = 17000; break;
        case B1200: conn->byte_usec = 8400;  break;
        case B2400: conn->byte_usec = 4200;  break;
        case B9600: conn->byte_usec = 1100;  break;
        case B19200:conn->byte_usec = 530;   break;
        case B38400:conn->byte_usec = 270;   break;
        case B57600:conn->byte_usec = 
    }
}

/*
* Description : Read's data from a connection's TTY.
*
* Arguments : conn - Connection pointer.
*
* Return Value : Zero if successful.
*                Non-zero if read() failed.
*/
extern int tty_read(CONN *conn)
{
    if (buf_read(conn->tty_fd, conn->tty_in) < 0) {
        log_msg(LOG_ERR, "%s:%d [%s] Error reading from TTY : %s\n", __FILE__,
                __LINE__, conn->name, strerror(errno));
        return -1;
    }
    log_msg(LOG_DEBUG, "%s:%d [%s] %u byte(s) received from TTY.\n", __FILE__,
            __LINE__, conn->name, conn->tty_in->len);
    return 0;
}

/*
* Description : Writes data to a connection's TTY.
*
* Arguments : conn - Connection pointer.
*
* Return Value : Zero if successful.
*                Non-zero if write() failed.
*/
extern int tty_write(CONN *conn)
{
    ssize_t len = buf_write(conn->tty_fd, conn->tty_out);
    if (len < 0) {
        log_msg(LOG_ERR,
                "%s:%d [%s] Error writing to TTY : %s\n", __FILE__, __LINE__,
                conn->name, strerror(errno));
        return -1;
    }
    log_msg(LOG_DEBUG, "%s:%d [%s] Wrote %u byte(s) to TTY.\n", __FILE__,
            __LINE__, conn->name, len);
    if (!conn->tty_out->len) tx_data_sent(conn);
    return 0;
}

/*
* Description : Closes a connection's TTY.
*
* Arguments : conn - Connection ponter.
*
* Return Value : None.
*/
extern void tty_close(CONN *conn)
{
    log_msg(LOG_DEBUG, "%s:%d [%s] Closing TTY.\n", __FILE__, __LINE__,
            conn->name);
    buf_free(conn->tty_in);
    buf_free(conn->tty_out);
again:
    if (close(conn->tty_fd)) {
        if (errno == EINTR) goto again;
        log_msg(LOG_ERR, "%s:%d [%s] Error closing TTY : %s\n", __FILE__,
                __LINE__, conn->name, strerror(errno));
    }
    return;
}

/*
* Description : Sets terminal options.
*
* Arguments : conn - Connection pointer.
*             rate - TTY baud rate.
*
* Return Value : Zero upon success.
*                Non-zero if an error occured.
*/
static int set_topts(const CONN *conn, int rate)
{
    struct termios topts;
    if (tcgetattr(conn->tty_fd, &topts))
    {
        log_msg(LOG_ERR,
                "%s:%d [%s] Failed to retrieve terminal attributes : %s\n",
                __FILE__, __LINE__, conn->name, strerror(errno));
        return -1;
    }
    if (cfsetspeed(&topts, rate))
    {
        log_msg(LOG_ERR, "%s:%d [%s] Failed to set TTY baud rate : %s\n",
                __FILE__, __LINE__, conn->name, strerror(errno));
        return -1;
    }
    topts.c_cflag &= ~PARENB; /* No parity. */
    topts.c_cflag &= ~CSIZE; /* 8 bits per byte. */
    topts.c_cflag |= CS8;
    topts.c_cflag &= ~CSTOPB; /* 1 stop bit. */
    topts.c_lflag &= ~ICANON; /* Disable canonical input mode. */
    topts.c_lflag &= ~ECHO; /* Disable echo. */
    topts.c_lflag &= ~ECHOE; /* Disable erase character echo. */
    topts.c_lflag &= ~ISIG; /* Disable signals */
    topts.c_iflag &= ~IXON; /* Disable output flow control. */
    topts.c_iflag &= ~IXOFF; /* Disable input flow control. */
    topts.c_iflag &= ~IXANY; /* Disable output restart characters. */
    topts.c_iflag &= ~ICRNL; /* Disable carrage return remapping. */
    topts.c_iflag &= ~INLCR; /* Disable newline remapping. */
    topts.c_oflag &= ~OPOST; /* Disable output postprocessing. */
    /*
    * Set the minimum bytes and timeout period for the read function.
    */
    topts.c_cc[VMIN] = 1;
    topts.c_cc[VTIME] = 0;
again:
    if (tcsetattr(conn->tty_fd, TCSANOW, &topts))
    {
        if (errno == EINTR) goto again;
        log_msg(LOG_ERR, "%s:%d [%s] Failed to set termial attributes : %s\n",
                __FILE__, __LINE__, conn->name, strerror(errno));
        return -1;
    }
    return 0;
}

/*
* Description : Allocates buffers for raw TTY I/O.
*
* Arguments : conn - Connection pointer.
*
* Return Value : Zero upon success.
*                Non-zero if a memory allocation error occured.
*/
static int alloc_bufs(CONN *conn)
{
    conn->tty_in = buf_new(TTY_BUF_SIZE);
    if (conn->tty_in == NULL) {
        throw RtlException(Serial$NotEnoughMemoryException, strerror(errno), errno);
        
        log_msg(LOG_ERR, "%s:%d [%s] Error allocating TTY buffers : %s\n",
                __FILE__, __LINE__, conn->name, strerror(errno));
        return -1;
    }
    conn->tty_out = buf_new(TTY_BUF_SIZE);
    if (conn->tty_out == NULL) {
        buf_free(conn->tty_in);
        log_msg(LOG_ERR, "%s:%d [%s] Error allocating TTY buffers : %s\n",
                __FILE__, __LINE__, conn->name, strerror(errno));
        return -1;
    }
    return 0;
}

} // namespace rtl