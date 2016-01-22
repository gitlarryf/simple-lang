#include <iso646.h>
#include <io.h>
#include <stdint.h>
#include <string>
//#include <WinBase.h>
#include <windows.h>

#include "rtl_exec.h"

#include "enums.inc"

class Port {
public:
    Port(): name(""), carrier_detect(false), clear_to_send(false), data_set_ready(false), data_terminal_ready(false), receive_signal_detect(false), carrier_dropped(false),
        use_parity(false), dsr_sensitivity(false), xon_continue(false), discard_nulls(false), abort_on_error(false), error_replacement(false), xon_char(17), xoff_char(19),
        error_char(0), eof_char(0), evt_char(0), xon_limit(128), xoff_limit(128), evt_flags(0), serial_status(0) {
        buf = NULL;
    };

    std::string name;
    HANDLE port;
    Number baud;
    Number stop;
    Number pairty;
    Number databits;

    // Flow Control
    Number RTSCTSControl;
    Number DSRDTRControl;
    Number XonXOff;

    // Serial connection line statuses
    bool carrier_detect;
    bool clear_to_send;
    bool data_set_ready;
    bool data_terminal_ready;
    bool receive_signal_detect;
    bool ring_indicator;

    bool carrier_dropped;

    // Serial Port Properties
    bool use_parity;
    bool dsr_sensitivity;
    bool xon_continue;
    bool discard_nulls;
    bool abort_on_error;
    bool error_replacement;

    uint8_t  xon_char;
    uint8_t  xoff_char;
    uint8_t  error_char;
    uint8_t  eof_char;
    uint8_t  evt_char;
    uint16_t xon_limit;
    uint16_t xoff_limit;

    // Serial line event flags to momitor
    uint32_t evt_flags;
    uint32_t serial_status;

    char *buf;          // Read data buffer...
};

static Port *check_port(void *pp)
{
    Port *p = static_cast<Port *>(pp);
    if (p == NULL) {
        throw RtlException(Exception_serial$InvalidPortException, "Null pointer dereference error.");
    }
    return p;
}

static void handle_error(DWORD error, const std::string &port)
{
    switch (error) {
        case ERROR_ALREADY_EXISTS: 
        case ERROR_ACCESS_DENIED:
            throw RtlException(Exception_serial$PortInUseException, port);
        case ERROR_PATH_NOT_FOUND: 
            throw RtlException(Exception_serial$InvalidPortException, port);
        default:
            throw RtlException(Exception_serial$PortNotOpenedException, port + ": " + std::to_string(error));
    }
}

//static uint32_t get_baud(Cell baud)
//{
//    switch(number_to_uint32(baud.number())) {
//        case ENUM_BaudRate_110BPS:       return 110;
//        case ENUM_BaudRate_300BPS:       return 300;
//        case ENUM_BaudRate_600BPS:       return 600;
//        case ENUM_BaudRate_1200BPS:      return 1200;
//        case ENUM_BaudRate_2400BPS:      return 2400;
//        case ENUM_BaudRate_4800BPS:      return 4800;
//        case ENUM_BaudRate_9600BPS:      return 9600;
//        case ENUM_BaudRate_14400BPS:     return 14400;
//        case ENUM_BaudRate_19200BPS:     return 19200;
//        case ENUM_BaudRate_38400BPS:     return 38400;
//        case ENUM_BaudRate_57600BPS:     return 57600;
//        case ENUM_BaudRate_115200BPS:    return 115200;
//        case ENUM_BaudRate_230400BPS:    return 230400;
//        case ENUM_BaudRate_460800BPS:    return 460800;
//        case ENUM_BaudRate_921600BPS:    return 921600;
//    }
//    throw RtlException(Exception_serial$UnsupportedBaudRateException, "The selected baud rate of " + std::to_string(number_to_uint32(baud.number())) + " is not supported.");
//}

static void update_port_status(void *pp)
{
    Port *p = check_port(pp);

    if (p->port == INVALID_HANDLE_VALUE) {
        return;
    }

    DWORD status;
    if (GetCommModemStatus(p->port, &status)) {
        p->data_set_ready           = (status & MS_DSR_ON) != 0;
        p->clear_to_send            = (status & MS_CTS_ON) != 0;
        p->ring_indicator           = (status & MS_RING_ON) != 0;
        p->receive_signal_detect    = (status & MS_RLSD_ON) != 0;

        bool had_carrier = p->carrier_detect;
        p->carrier_detect           = (status & MS_RLSD_ON) != 0;

        if (had_carrier && not p->carrier_detect) {
            p->carrier_dropped = true;
        }

        if(status != p->serial_status) {
            // TODO: Signal that the line status has changed.
            p->serial_status = status;
        }
    }
}

void setup_comm_state(void *pp)
{
    Port *p = check_port(pp);

    if (p->port == INVALID_HANDLE_VALUE) {
        return;
    }

    DCB dcb;
    if (not GetCommState(p->port, &dcb)) {
        DWORD e = GetLastError();
        throw RtlException(Exception_serial$GetPortSettingsException, "Failed to setup comm state for port: " + p->name + ".  Error: " + std::to_string(e) + ".", e);
    }

    dcb.BaudRate = number_to_uint32(p->baud);
    dcb.fBinary = TRUE;

    // Setup parity
    BYTE parity = 0;
    p->use_parity = true;
    switch (number_to_uint32(p->pairty)) {
        case ENUM_Parity_None: 
            p->use_parity = false;
            parity = NOPARITY;
            break;
        case ENUM_Parity_Odd:   parity = ODDPARITY;     break;
        case ENUM_Parity_Even:  parity = EVENPARITY;    break;
        case ENUM_Parity_Mark:  parity = MARKPARITY;    break;
        case ENUM_Parity_Space: parity = SPACEPARITY;   break;
    }
    dcb.Parity = parity;
    dcb.fParity = p->use_parity ? TRUE : FALSE;

    // Setup data bits
    BYTE databits = 0;
    switch (number_to_uint32(p->databits)) {
        case ENUM_DataBits_Five:    databits = 5; break;
        case ENUM_DataBits_Six:     databits = 6; break;
        case ENUM_DataBits_Seven:   databits = 7; break;
        case ENUM_DataBits_Eight:   databits = 8; break;
        default: 
            databits = 8;     break;
    }
    dcb.ByteSize = databits;

    // Setup stop bits
    BYTE stopbits = 0;
    switch (number_to_uint32(p->stop)) {
        case ENUM_StopBits_One:         stopbits = ONESTOPBIT;  break;
        case ENUM_StopBits_OnePointFive:stopbits = ONE5STOPBITS;break;
        case ENUM_StopBits_Two:         stopbits = TWOSTOPBITS; break;
        //case 3: stopbits = 8; break;
        default: 
            stopbits = 8;     break;
    }
    dcb.StopBits = stopbits;

    // Setup Flow Control
    if (number_to_uint32(p->XonXOff) == 1) {
        dcb.fInX = TRUE;
        dcb.fOutX = TRUE;
    } else {
        dcb.fInX = FALSE;
        dcb.fOutX = FALSE;
    }
    if (number_to_uint32(p->RTSCTSControl) == 1) {
        dcb.fOutxCtsFlow = TRUE;
    } else {
        dcb.fOutxCtsFlow = FALSE;
    }
    if (number_to_uint32(p->DSRDTRControl) == 1) {
        dcb.fOutxDsrFlow = TRUE;
    } else {
        dcb.fOutxDsrFlow = FALSE;
    }
    switch (number_to_uint32(p->DSRDTRControl)) {
        case 0: dcb.fOutxDsrFlow = FALSE; break;
        case 1: dcb.fOutxDsrFlow = TRUE; break;
    }
    switch (number_to_uint32(p->DSRDTRControl)) {
        case 0: dcb.fDtrControl = DTR_CONTROL_DISABLE;  break;
        case 1: dcb.fDtrControl = DTR_CONTROL_ENABLE;   break;
        case 2: dcb.fDtrControl = DTR_CONTROL_HANDSHAKE;break;
    }
    switch (number_to_uint32(p->RTSCTSControl)) {
        case 0: dcb.fRtsControl = RTS_CONTROL_DISABLE;  break;
        case 1: dcb.fRtsControl = RTS_CONTROL_ENABLE;   break;
        case 2: dcb.fRtsControl = RTS_CONTROL_HANDSHAKE;break;
        case 3: dcb.fRtsControl = RTS_CONTROL_TOGGLE;   break;
    }

    // Setup Comm behaviours
    dcb.fErrorChar = FALSE;
    if (p->error_replacement) {
        if (p->error_char) {
            dcb.fErrorChar = TRUE;
            dcb.ErrorChar = p->error_char;
        }
    }
    dcb.fNull = FALSE;
    if (p->discard_nulls) {
        dcb.fNull = TRUE;
    }

    COMMPROP prop;
    if (number_to_uint16(p->XonXOff)) {
        if (not GetCommProperties(p->port, &prop)) {
            DWORD e = GetLastError();
            throw RtlException(Exception_serial$GetPortSettingsException, "Failed to setup comm state for port: " + p->name + ".  Error: " + std::to_string(e) + ".", e);
        }
        dcb.XoffLim = WORD(prop.dwCurrentRxQueue / 4);  // Shutdown when the buffer is 3/4 full.
        dcb.XonLim = WORD(prop.dwCurrentRxQueue / 4);   // Resume when it's only 1/4 is full.
    }

    dcb.fAbortOnError = FALSE;

    SetCommState(p->port, &dcb);
    SetCommMask(p->port, EV_BREAK | EV_DSR | EV_RING | EV_RLSD | EV_ERR);

    COMMTIMEOUTS cto;
    cto.ReadIntervalTimeout = 50;
    cto.ReadTotalTimeoutMultiplier = 0;
    cto.ReadTotalTimeoutConstant = 100;
    cto.WriteTotalTimeoutMultiplier = 0;
    cto.WriteTotalTimeoutConstant = 0;
    
    if (not SetCommTimeouts(p->port, &cto)) {
        DWORD e = GetLastError();
        throw RtlException(Exception_serial$SetPortTimeoutsException, "Failed to set comm timeouts for port: " + p->name + ".  Error: " + std::to_string(e) + ".", e);
    }

    update_port_status(pp);
}

void flush_buffer(void *pp)
{
    Port *p = check_port(pp);

    if (p->port == INVALID_HANDLE_VALUE) {
        return;
    }
    
    PurgeComm(p->port, PURGE_TXCLEAR);
}

void set_port(void *pp, Cell baud, Cell parity, Cell databits, Cell stopbits)
{
    Port *p = check_port(pp);

    if(p->port == INVALID_HANDLE_VALUE) {
        return;
    }
    
    DCB dcb;
    GetCommState(p->port, &dcb);
    
    dcb.BaudRate = number_to_uint32(p->baud);
    //dcb.Parity = parity;
    //dcb.ByteSize = databits;
    //dcb.StopBits = stopbits;
    
    SetCommState(p->port, &dcb);
}

//void CSerialIO::GetLine(DWORD &baud, BYTE &parity, BYTE &databits, BYTE &stopbits)
//{
//    if(m_hCom == INVALID_HANDLE_VALUE) {
//        return;
//    }
//
//    DCB dcb;
//    GetCommState(m_hCom, &dcb);
//    
//    baud = dcb.BaudRate;
//    parity = dcb.Parity;
//    databits = dcb.ByteSize;
//    stopbits = dcb.StopBits;
//}

void set_rts_line(void *pp, Cell *on)
{
    Port *p = check_port(pp);

    if (p->port == INVALID_HANDLE_VALUE) {
        return;
    }

    if (not EscapeCommFunction(p->port, on->boolean() ? SETRTS : CLRRTS)) {
        throw RtlException(Exception_serial$SetCommLineException, "Failed to " + std::string((on->boolean() ? "set" : "clear")) + " the RTS line.");
    }
}

//void CSerialIO::HWFlowEnable(DWORD options)
//{
//    if(m_hCom == INVALID_HANDLE_VALUE) {
//        return;
//    }
//
//    DCB dcb;
//    GetCommState(m_hCom, &dcb);
//    
//    if((options & hfUseRTS) == hfUseRTS) {
//        dcb.fRtsControl = RTS_CONTROL_HANDSHAKE;
//    } else {
//        dcb.fRtsControl = RTS_CONTROL_ENABLE;
//    }
//    
//    if((options & hfUseDTR) == hfUseDTR) {
//        dcb.fDtrControl = DTR_CONTROL_HANDSHAKE;
//    } else {
//        dcb.fDtrControl = DTR_CONTROL_ENABLE;
//    }
//    
//    COMMPROP prop;
//    GetCommProperties(m_hCom, &prop);
//    
//    dcb.XoffLim = WORD(prop.dwCurrentRxQueue / 4);  // Shutdown when the buffer is 3/4 full...
//    dcb.XonLim = WORD(prop.dwCurrentRxQueue / 4);   // Resume when the buffer is only 1/4 is full...
//    dcb.fOutxCtsFlow = ((options & hfRequireCTS) == hfRequireCTS);
//    dcb.fOutxDsrFlow = ((options & hfRequireDSR) == hfRequireDSR);
//    
//    SetCommState(m_hCom, &dcb);
//}
//
//void CSerialIO::HWFlowDisable()
//{
//    if(m_hCom == INVALID_HANDLE_VALUE) {
//        return;
//    }
//
//    DCB dcb;
//    GetCommState(m_hCom, &dcb);
//    
//    dcb.fRtsControl = RTS_CONTROL_ENABLE;
//    dcb.fDtrControl = DTR_CONTROL_ENABLE;
//    dcb.fOutxCtsFlow = FALSE;
//    dcb.fOutxDsrFlow = FALSE;
//    
//    SetCommState(m_hCom, &dcb);
//}
//
//void CSerialIO::SWFlowEnable()
//{
//    if(m_hCom == INVALID_HANDLE_VALUE) {
//        return;
//    }
//
//    DCB dcb;
//    GetCommState(m_hCom, &dcb);
//
//    dcb.XonChar = 17;
//    dcb.XoffChar = 19;
//
//    COMMPROP prop;
//    GetCommProperties(m_hCom, &prop);
//
//    dcb.XoffLim = WORD(prop.dwCurrentRxQueue / 4);  // Shutdown when the buffer is 3/4 full...
//    dcb.XonLim = WORD(prop.dwCurrentRxQueue / 4);   // Resume when the buffer is only 1/4 is full...
//    dcb.fOutX = TRUE;
//    dcb.fInX = TRUE;
//
//    SetCommState(m_hCom, &dcb);
//}
//
//void CSerialIO::SWFlowDisable()
//{
//    if(m_hCom == INVALID_HANDLE_VALUE) {
//        return;
//    }
//    
//    DCB dcb;
//    GetCommState(m_hCom, &dcb);
//    
//    dcb.fOutX = FALSE;
//    dcb.fInX = FALSE;
//    
//    SetCommState(m_hCom, &dcb);
//}

namespace rtl {

void *serial$open(const std::string &name, Number baud, Cell &softwareFlowControl, Cell &hardwareFlowControl)
{
    Port *p = new Port;
    p->name.append(name);
    p->baud = baud;
    p->buf = new char[16384];           // Default to a 16k buffer...

    p->carrier_detect           = false;
    p->data_set_ready           = false;
    p->data_terminal_ready      = false;
    p->carrier_dropped          = false;
    switch (number_to_uint32(softwareFlowControl.number())) {
        case ENUM_FlowControl_Disable:
            p->XonXOff                  = number_from_uint32(ENUM_FlowControl_Disable);
            break;
        case ENUM_FlowControl_Enable:
        case ENUM_FlowControl_DSR_DTR:
        case ENUM_FlowControl_RTS_CTS:
        case ENUM_FlowControl_Handshake:
        case ENUM_FlowControl_Toggle:
            break;
            // ToDo: Finish Flow control impl.
    }

    switch (number_to_uint32(hardwareFlowControl.number())) {
        case ENUM_FlowControl_Disable:
            p->RTSCTSControl            = number_from_uint32(ENUM_FlowControl_Disable);
            p->DSRDTRControl            = number_from_uint32(ENUM_FlowControl_Disable);
            break;
        case ENUM_FlowControl_Enable:
        case ENUM_FlowControl_DSR_DTR:
        case ENUM_FlowControl_RTS_CTS:
        case ENUM_FlowControl_Handshake:
        case ENUM_FlowControl_Toggle:
            break;
            // ToDo: Finish Flow control impl.
    }

    DCB dcb;

    //  Open a handle to the specified com port.
    p->port = CreateFile(name.c_str(), GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
    if (p->port == INVALID_HANDLE_VALUE) {
        DWORD e = GetLastError();
        delete p->buf;
        delete p;
        handle_error(e, "Failed to open port " + name + ".  Error: " + std::to_string(e) + ".");
    }

    ZeroMemory(&dcb, sizeof(DCB));
    dcb.DCBlength = sizeof(DCB);

    if (not GetCommState(p->port, &dcb)) {
        DWORD e = GetLastError();
        throw RtlException(Exception_serial$GetPortSettingsException, "Failed to get port properties for " + name + ".  Error: " + std::to_string(e) + ".", e);
    }
    return p;
}

void serial$setDataTerminalReadyLine(Cell **pp, Cell *on)
{
    Port *p = check_port(pp);

    if (p->port == INVALID_HANDLE_VALUE) {
        return;
    }

    DCB dcb;
    if (not GetCommState(p->port, &dcb)) {
        DWORD e = GetLastError();
        throw RtlException(Exception_serial$GetPortSettingsException, "Failed to get port settings for " + p->name + ".  Error " + std::to_string(e), e);
    }

    dcb.fDtrControl = on->boolean() ? DTR_CONTROL_ENABLE : DTR_CONTROL_DISABLE;

    if (not SetCommState(p->port, &dcb)) {
        DWORD e = GetLastError();
        throw RtlException(Exception_serial$SetPortSettingsException, "Failed to set port settings for " + p->name + ".  Error " + std::to_string(e), e);
    }
}

std::string serial$read(void *pp)
{
    Port *p = check_port(pp);
    DWORD read = 16384;
    DWORD got = 0;

    if (ReadFile(p->port, p->buf, read, &got, NULL)) {
        return std::string(reinterpret_cast<char*>(p->buf), got);
    }
    //DWORD e = GetLastError();
    //throw RtlException(Exception_serial$ReadPortException, "Failed to read data from port " + p->name + ". Error: " + std::to_string(e) + ".", e);
    return std::string("");
}

bool serial$write(void *pp, const std::string &data)
{
    Port *p = check_port(pp);
    DWORD wrote = 0;

    if(not WriteFile(p->port, data.data(), static_cast<DWORD>(data.length()), &wrote, NULL)) {
        DWORD e = GetLastError();
        throw RtlException(Exception_serial$WritePortException, "Failed to write data to port " + p->name + ". Error: " + std::to_string(e) + ".", e);
    }
    return data.length() == wrote;
}

void serial$close(Cell **pp)
{
    Port *p = check_port(pp);
    CloseHandle(p->port);
    delete p->buf;
    delete pp;
}

}
