#include "number.h"

typedef void (*void_function_t)();

void_function_t rtl_external_function(const std::string &library, const std::string &function, std::string &exception);

Number rtl_time_now();
