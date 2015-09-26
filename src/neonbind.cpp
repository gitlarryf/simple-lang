#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS
#endif
#include <fstream>
#include <iso646.h>
#include <map>
#include <sstream>
#include <stdio.h>
#include <string>
#include <sys/stat.h>
#include <vector>

#ifdef _MSC_VER
#define unlink _unlink
#endif

#include <zip.h>

#include "support.h"

#ifdef _WIN32
#define STUB_NAME "neonstub.exe"
#else
#define STUB_NAME "neonstub"
#endif

RuntimeSupport support(".");

void get_modules(const Bytecode &obj, std::map<std::string, Bytecode> &modules)
{
    for (auto x: obj.imports) {
        std::string name = obj.strtable[x.first];
        if (modules.find(name) == modules.end()) {
            Bytecode bytecode;
            if (support.loadBytecode(name, bytecode)) {
                modules[name] = bytecode;
                get_modules(bytecode, modules);
            } else {
                fprintf(stderr, "could not load module: %s\n", name.c_str());
                exit(1);
            }
        }
    }
}

int main(int argc, char *argv[])
{
    if (argc < 3) {
        fprintf(stderr, "Usage: %s executable module\n", argv[0]);
        exit(1);
    }

    std::map<std::string, Bytecode> modules;
    {
        std::ifstream inf(argv[2], std::ios::binary);
        if (not inf.good()) {
            fprintf(stderr, "could not find: %s\n", argv[2]);
            exit(1);
        }
        std::stringstream buf;
        buf << inf.rdbuf();
        std::vector<unsigned char> bytecode;
        std::string s = buf.str();
        std::copy(s.begin(), s.end(), std::back_inserter(bytecode));
        modules[""] = Bytecode(bytecode);
    }
    get_modules(modules[""], modules);

    const std::string zipname = std::string(argv[1]) + ".zip";
    {
        zipFile zip = zipOpen(zipname.c_str(), 0);
        if (zip == NULL) {
            fprintf(stderr, "zip open error\n");
            exit(1);
        }
        for (auto m: modules) {
            int r = zipOpenNewFileInZip(zip, (m.first+".neonx").c_str(), NULL, NULL, 0, NULL, 0, NULL, Z_DEFLATED, Z_DEFAULT_COMPRESSION);
            if (r != ZIP_OK) {
                fprintf(stderr, "zip open error\n");
                exit(1);
            }
            r = zipWriteInFileInZip(zip, m.second.obj.data(), static_cast<unsigned int>(m.second.obj.size()));
            zipCloseFileInZip(zip);
        }
        zipClose(zip, NULL);
    }

    FILE *stub = fopen("bin/" STUB_NAME, "rb");
    if (stub == NULL) {
        fprintf(stderr, "stub open\n");
        exit(1);
    }
    FILE *zip = fopen(zipname.c_str(), "rb");
    if (zip == NULL) {
        fprintf(stderr, "zip open\n");
        exit(1);
    }
    FILE *exe = fopen(argv[1], "wb");
    if (exe == NULL) {
        fprintf(stderr, "exe open\n");
        exit(1);
    }
    for (;;) {
        char buf[4096];
        size_t n = fread(buf, 1, sizeof(buf), stub);
        if (n == 0) {
            break;
        }
        fwrite(buf, 1, n, exe);
    }
    for (;;) {
        char buf[4096];
        size_t n = fread(buf, 1, sizeof(buf), zip);
        if (n == 0) {
            break;
        }
        fwrite(buf, 1, n, exe);
    }
    fclose(stub);
    fclose(zip);
    #ifndef _WIN32
        fchmod(fileno(exe), 0755);
    #endif
    fclose(exe);
    unlink(zipname.c_str());
}