#include "number.h"

namespace rtl {

const Number serial$EVENT_BREAK      = number_from_uint32(0x0001);      // A break was detected on input.
const Number serial$EVENT_CTS        = number_from_uint32(0x0002);      // The CTS (clear-to-send) signal changed state.
const Number serial$EVENT_DSR        = number_from_uint32(0x0004);      // The DSR (data-set-ready) signal changed state.
const Number serial$EVENT_ERROR      = number_from_uint32(0x0008);      // A line-status error occurred. Line-status errors would be one of:  CE_FRAME, CE_OVERRUN, or CE_RXPARITY.
const Number serial$EVENT_RING       = number_from_uint32(0x0010);      // A ring indicator was detected.
const Number serial$EVENT_RSLD       = number_from_uint32(0x0020);      // The RLSD (receive-line-signal-detect) signal changed state.
const Number serial$EVENT_TXEMPTY    = number_from_uint32(0x0040);      // The last character in the output buffer was sent.

} // namespace rtl
