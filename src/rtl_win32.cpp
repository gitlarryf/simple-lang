#include "rtl_platform.h"

#include <map>
#include <windows.h>

static std::map<std::string, HMODULE> g_Libraries;

static HMODULE get_library_handle(const std::string &library)
{
    auto i = g_Libraries.find(library);
    if (i == g_Libraries.end()) {
        HMODULE lib = LoadLibrary(library.c_str());
        if (lib == NULL) {
            return NULL;
        }
        i = g_Libraries.insert(std::make_pair(library, lib)).first;
    }
    return i->second;
}

void_function_t rtl_external_function(const std::string &library, const std::string &function, std::string &exception)
{
    HMODULE lib = get_library_handle(library);
    if (lib == NULL) {
        exception = "LibraryNotFound";
        return nullptr;
    }
    void_function_t fp = reinterpret_cast<void_function_t>(GetProcAddress(lib, function.c_str()));
    if (fp == NULL) {
        exception = "FunctionNotFound";
        return nullptr;
    }
    return fp;
}
