#ifndef __STDUTIL_H_
#define __STDUTIL_H_

#include <cstdlib>
#include <sstream>
#include <string>

namespace std {

template <typename T> std::string to_string(T x) {
    std::stringstream buf;
    buf << x;
    return buf.str();
};

unsigned long stoul(const std::string &str, std::size_t *pos = 0, int base = 10);
int stoi(const std::string &str, std::size_t *pos = 0, int base = 10);

} // namespace std

#endif // __STDUTIL_H_

