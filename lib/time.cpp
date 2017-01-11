#include <thread>
#include <time.h>
#include <sys/select.h>
#include <sys/time.h>

#include "number.h"
#include "rtl_platform.h"

namespace rtl {

namespace time {

void sleep(Number seconds)
{
#if (defined(__arm__) && defined(__VERSION__)) 
#if _POSIX_C_SOURCE >= 199309L
    struct timespec ts;
    ts.tv_sec = number_to_uint64(number_subtract(number_trunc(seconds), seconds));
    ts.tv_nsec = number_to_uint64(number_subtract(number_multiply(seconds, number_from_uint32(1000000)), number_trunc(number_multiply(seconds, number_from_uint32(1000000))))); 
    int ret = nanosleep(&ts, NULL);
#else
    struct timeval tv;
    tv.tv_usec = number_to_uint64(number_subtract(number_multiply(seconds, number_from_uint32(1000000)), number_trunc(number_multiply(seconds, number_from_uint32(1000000))))); 
    tv.tv_sec = number_to_uint64(number_subtract(number_trunc(seconds), seconds));
    int ret = slect(0, NULL, NULL, NULL, &tv);
#endif
#else
    std::chrono::microseconds us(number_to_uint64(number_multiply(seconds, number_from_uint32(1000000))));
    std::this_thread::sleep_for(us);
#endif
}

} // namespace time

} // namespace rtl
