#include "support.h"

#include <fstream>
#include <iso646.h>
#include <sstream>

#include "bytecode.h"

#include "rtlx.inc"

bool RuntimeSupport::loadBytecode(const std::string &name, Bytecode &object)
{
    for (size_t i = 0; i < sizeof(rtl_bytecode)/sizeof(rtl_bytecode[0]); i++) {
        if (name == rtl_bytecode[i].name) {
            std::vector<unsigned char> bytecode {rtl_bytecode[i].bytecode, rtl_bytecode[i].bytecode + rtl_bytecode[i].length};
            object.load("-builtin-", bytecode);
            return true;
        }
    }

    std::pair<std::string, std::string> names = findModule(name);
    std::ifstream inf(names.second, std::ios::binary);
    if (not inf.good()) {
        return false;
    }
    std::stringstream buf;
    buf << inf.rdbuf();
    std::vector<unsigned char> bytecode;
    std::string s = buf.str();
    std::copy(s.begin(), s.end(), std::back_inserter(bytecode));
    if (not object.load(names.second, bytecode)) {
        return false;
    }
    return true;
}
