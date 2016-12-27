#include "rtl_compile.h"

#include "functions_compile.inc"
#include "variables_compile.inc"

static const ast::Type *resolve_type(const PredefinedType &ptype, ast::Scope *scope)
{
    auto *type = ptype.type;
    if (type == nullptr && scope != nullptr) {
        ast::Name *name = scope->lookupName(ptype.modtypename);
        type = dynamic_cast<const ast::Type *>(name);
    }
    return type;
}

void rtl_compile_init(ast::Scope *scope)
{
    //init_builtin_constants(scope);
    for (unsigned int f = 0; f < (sizeof(BuiltinFunctions) / sizeof(BuiltinFunctions[0])); f++) {
        std::vector<const ast::ParameterType *> params;
        for (int i = 0; i < BuiltinFunctions[f].count; i++) {
            auto &p = BuiltinFunctions[f].params[i];
            params.push_back(new ast::ParameterType(Token(p.name), p.mode, resolve_type(p.ptype, nullptr), nullptr));
        }
        scope->addName(Token(IDENTIFIER, BuiltinFunctions[f].name), BuiltinFunctions[f].name, new ast::PredefinedFunction(BuiltinFunctions[f].name, new ast::TypeFunction(resolve_type(BuiltinFunctions[f].returntype, nullptr), params)));
    }
}

bool rtl_import(const std::string &module, ast::Module *mod)
{
    std::string prefix = module + "$";
    init_builtin_variables(module, mod->scope);
    bool any = false;
    for (size_t f = 0; f < (sizeof(BuiltinFunctions) / sizeof(BuiltinFunctions[0])); f++) {
        std::string qualified_name(BuiltinFunctions[f].name);
        if (BuiltinFunctions[f].exported && qualified_name.substr(0, prefix.length()) == prefix) {
            std::vector<const ast::ParameterType *> params;
            for (size_t i = 0; i < (sizeof(BuiltinFunctions) / sizeof(BuiltinFunctions[0])); i++) {
                auto p = BuiltinFunctions[f].params[i];
                params.push_back(new ast::ParameterType(Token(p.name), p.mode, resolve_type(p.ptype, mod->scope), nullptr));
            }
            mod->scope->addName(Token(IDENTIFIER, BuiltinFunctions[f].name), qualified_name.substr(prefix.length()), new ast::PredefinedFunction(BuiltinFunctions[f].name, new ast::TypeFunction(resolve_type(BuiltinFunctions[f].returntype, mod->scope), params)));
            any = true;
        }
    }
    return any;
}
