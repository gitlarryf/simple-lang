#include <cstdlib>
#include <sstream>
#include <string>

namespace std {

unsigned long stoul(const std::string &str, std::size_t *pos = 0, int base = 10)
{
    const char *p = str.c_str();
    char *end;
    unsigned long n = strtoul(p, &end, base);
    if (pos) {
        *pos = end - p;
    }
    return n;
}

int stoi(const std::string &str, std::size_t *pos = 0, int base = 10)
{
    const char *p = str.c_str();
    char *end;
    int n = std::strtol(p, &end, base);
    if (pos) {
        *pos = end - p;
    }
    return n;
}

} // namespace std
