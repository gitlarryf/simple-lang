import os
import subprocess
import sys

ExcludeTests = [
    "t/42.neon",
    "t/arithmetic2.neon",
    "t/arithmetic.neon",
    "t/array2d.neon",
    "t/array-append.neon",
    "t/array-concat.neon",
    "t/array-extend.neon",
    "t/array-fraction.neon",
    "t/array-index-write.neon",
    "t/array-last-method.neon",
    "t/array-negative.neon",
    "t/array.neon",
    "t/array-resize.neon",
    "t/arraysize.neon",
    "t/array-slice.neon",
    "t/array-subscript.neon",
    "t/array-tostring.neon",
    "t/assert-empty-array.neon",
    "t/assert-enum.neon",
    "t/assert-fail2.neon",
    "t/assert-fail.neon",
    "t/assert.neon",
    "t/assign2.neon",
    "t/assign-ignore.neon",
    "t/assignment.neon",
    "t/assign.neon",
    "t/assign-nothing.neon",
    "t/base-decimal.neon",
    "t/base-invalidchar.neon",
    "t/base-invalid-digit.neon",
    "t/base-invalid.neon",
    "t/base.neon",
    "t/bigint.neon",
    "t/binary-test.neon",
    "t/boolean.neon",
    "t/bytecode-stringtable.neon",
    "t/bytes-embed.neon",
    "t/bytes-literal.neon",
    "t/bytes.neon",
    "t/bytes-slice.neon",
    "t/bytes-tostring.neon",
    "t/bytes-value-index.neon",
    "t/cal-test.neon",
    "t/case2.neon",
    "t/case3.neon",
    "t/case4.neon",
    "t/case.neon",
    "t/case-jumptbl.neon",
    "t/case-overlap.neon",
    "t/cformat-test.neon",
    "t/check-if.neon",
    "t/check.neon",
    "t/class-empty.neon",
    "t/cmdline.neon",
    "t/comments-block2.neon",
    "t/comments-block3.neon",
    "t/comments-block4.neon",
    "t/comments-block5.neon",
    "t/comments-block6.neon",
    "t/comments-block.neon",
    "t/comments.neon",
    "t/comparison2.neon",
    "t/comparison.neon",
    "t/complex-test.neon",
    "t/concat-bytes.neon",
    "t/concat.neon",
    "t/conditional.neon",
    "t/const-assign.neon",
    "t/const-boolean.neon",
    "t/const-chain.neon",
    "t/const-expression.neon",
    "t/const.neon",
    "t/const-notconst.neon",
    "t/const-string.neon",
    "t/datetime-test.neon",
    "t/debug-example.neon",
    "t/debug-server.neon",
    "t/decimal.neon",
    "t/dictionary-keys.neon",
    "t/dictionary-keys-tostring.neon",
    "t/dictionary.neon",
    "t/dictionary-sorted.neon",
    "t/divide-by-zero.neon",
    "t/duplicate.neon",
    "t/empty.neon",
    "t/encoding-base64.neon",
    "t/enum3.neon",
    "t/enum.neon",
    "t/equality.neon",
    "t/exception-as.neon",
    "t/exception-code.neon",
    "t/exception-function.neon",
    "t/exception.neon",
    "t/exception-nested.neon",
    "t/exception-opstack.neon",
    "t/exception-stackerror.neon",
    "t/exception-subexception.neon",
    "t/exception-trace.neon",
    "t/exit-for.neon",
    "t/exit-while.neon",
    "t/export-function-indent.neon",
    "t/export-inline.neon",
    "t/export.neon",
    "t/export-recursive.neon",
    "t/expr.neon",
    "t/ffi.neon",
    "t/file-exists.neon",
    "t/file-filecopied1.neon",
    "t/file-filecopied2.neon",
    "t/file-filecopied3.neon",
    "t/file-filecopied.neon",
    "t/file-symlink.neon",
    "t/file-test.neon",
    "t/file-writebytes.neon",
    "t/file-writelines.neon",
    "t/for-bounds.neon",
    "t/foreach-bytes.neon",
    "t/foreach-eval.neon",
    "t/foreach-function.neon",
    "t/foreach.neon",
    "t/foreach-string.neon",
    "t/foreach-update.neon",
    "t/foreach-value.neon",
    "t/fork.neon",
    "t/for.neon",
    "t/for-nested2.neon",
    "t/for-nested.neon",
    "t/for-readonly.neon",
    "t/for-scope.neon",
    "t/forth-test.neon",
    "t/forward.neon",
    "t/function-args.neon",
    "t/function-defaultargs.neon",
    "t/function-local.neon",
    "t/function-namedargs.neon",
    "t/function.neon",
    "t/function-pointer.neon",
    "t/function-pointer-nowhere.neon",
    "t/gc1.neon",
    "t/gc2.neon",
    "t/gc3.neon",
    "t/gc-array.neon",
    "t/gc-long-chain.neon",
    "t/global-shadow.neon",
    "t/if.neon",
    "t/import-dup.neon",
    "t/import.neon",
    "t/import-string.neon",
    "t/inc.neon",
    "t/inc-reference.neon",
    "t/indent.neon",
    "t/index.neon",
    "t/inline-construct-record.neon",
    "t/inline-init4.neon",
    "t/inline-init.neon",
    "t/inline-init-record.neon",
    "t/in.neon",
    "t/input.neon",
    "t/intdiv.neon",
    "t/interface.neon",
    "t/interface-parameter-export2.neon",
    "t/interface-parameter-export.neon",
    "t/interface-parameter-import2.neon",
    "t/interface-parameter-import.neon",
    "t/intrinsic.neon",
    "t/io-test.neon",
    "t/json-test.neon",
    "t/let-assign.neon",
    "t/let.neon",
    "t/lexer-raw.neon",
    "t/lexer-unicode.neon",
    "t/lexical-scope.neon",
    "t/lisp-test.neon",
    "t/literal-array.neon",
    "t/literal-dup.neon",
    "t/literal-empty.neon",
    "t/literal.neon",
    "t/literal-string.neon",
    "t/local-clear.neon",
    "t/logical2.neon",
    "t/logical.neon",
    "t/loop-label.neon",
    "t/loop.neon",
    "t/loop-return-foreach.neon",
    "t/loop-return-for.neon",
    "t/loop-return-loop.neon",
    "t/loop-return-repeat-infinite.neon",
    "t/loop-return-repeat.neon",
    "t/loop-return-while-infinite.neon",
    "t/loop-return-while.neon",
    "t/math-test.neon",
    "t/methods-declare.neon",
    "t/methods.neon",
    "t/methods-self.neon",
    "t/mismatch.neon",
    "t/mkdir.neon",
    "t/mmap-test.neon",
    "t/module2.neon",
    "t/module-alias2.neon",
    "t/module-alias.neon",
    "t/module-assign-let.neon",
    "t/module-assign-var.neon",
    "t/module-import-name2.neon",
    "t/module-import-name3.neon",
    "t/module-import-name-alias2.neon",
    "t/module-import-name-alias.neon",
    "t/module-import-name.neon",
    "t/module.neon",
    "t/module-scope.neon",
    "t/modulo.neon",
    "t/multiarray-test.neon",
    "t/nested-substitution.neon",
    "t/net-test.neon",
    "t/net-test-udp.neon",
    "t/new-init-module.neon",
    "t/new-init.neon",
    "t/next-for.neon",
    "t/next-while.neon",
    "t/number-ceil.neon",
    "t/number-underscore.neon",
    "t/object-isa-case.neon",
    "t/object-isa.neon",
    "t/object.neon",
    "t/object-null.neon",
    "t/object-operator.neon",
    "t/os-test.neon",
    "t/outer2.neon",
    "t/outer-issue192.neon",
    "t/outer.neon",
    "t/outer-parameter.neon",
    "t/outer-tail.neon",
    "t/parameter-inout-array.neon",
    "t/parameter-inout-string.neon",
    "t/parameter-out-array.neon",
    "t/parameter-out-string.neon",
    "t/parameters-ignore.neon",
    "t/parameters.neon",
    "t/pointer2.neon",
    "t/pointer3.neon",
    "t/pointer4.neon",
    "t/pointer5.neon",
    "t/pointer6.neon",
    "t/pointer7.neon",
    "t/pointer8.neon",
    "t/pointer-method.neon",
    "t/pointer-mismatch.neon",
    "t/pointer.neon",
    "t/pointer-nil.neon",
    "t/pointer-valid.neon",
    "t/predeclare1.neon",
    "t/predeclare2.neon",
    "t/predeclare3.neon",
    "t/predefined-generation.neon",
    "t/process-test.neon",
    "t/record-empty.neon",
    "t/record-init2.neon",
    "t/record-init.neon",
    "t/record-private.neon",
    "t/recursion-limit.neon",
    "t/recursion.neon",
    "t/repeat.neon",
    "t/repeat-next.neon",
    "t/repl_assign2.neon",
    "t/repl_assign.neon",
    "t/repl_constant.neon",
    "t/repl_enum_tostring.neon",
    "t/repl_for.neon",
    "t/repl_function.neon",
    "t/repl_import.neon",
    "t/return-case2.neon",
    "t/return-case.neon",
    "t/return-if.neon",
    "t/return-loop.neon",
    "t/return.neon",
    "t/return-nothing.neon",
    "t/return-try.neon",
    "t/retval-index.neon",
    "t/rtl.neon",
    "t/shadow2.neon",
    "t/shadow.neon",
    "t/shebang.neon",
    "t/shortcut.neon",
    "t/sql-connect.neon",
    "t/sql-cursor.neon",
    "t/sql-embed.neon",
    "t/sql-execute.neon",
    "t/sqlite-test.neon",
    "t/sql-query.neon",
    "t/sql-whenever.neon",
    "t/stack-overflow.neon",
    "t/string-bytes.neon",
    "t/string-escape.neon",
    "t/string-format.neon",
    "t/string-multiline.neon",
    "t/string-slice.neon",
    "t/strings.neon",
    "t/string-test.neon",
    "t/struct-test.neon",
    "t/structure.neon",
    "t/sudoku-test.neon",
    "t/sys-exit.neon",
    "t/tail-call.neon",
    "t/time-stopwatch.neon",
    "t/time-test.neon",
    "t/tostring.neon",
    "t/try-expression.neon",
    "t/type-compat.neon",
    "t/type_mismatch.neon",
    "t/type.neon",
    "t/type-nested.neon",
    "t/unary-plus.neon",
    "t/unicode-char.neon",
    "t/unicode-length.neon",
    "t/unicode-source.neon",
    "t/unicode-string.neon",
    "t/uninitialised-case.neon",
    "t/uninitialised-case-noelse.neon",
    "t/uninitialised-function.neon",
    "t/uninitialised-if-exit.neon",
    "t/uninitialised-if.neon",
    "t/uninitialised-if-nested.neon",
    "t/uninitialised-if-noelse.neon",
    "t/uninitialised-loop.neon",
    "t/uninitialised-nested.neon",
    "t/uninitialised-out.neon",
    "t/uninitialised-repeat.neon",
    "t/uninitialised-simple.neon",
    "t/uninitialised-try.neon",
    "t/unused-declaration.neon",
    "t/unused.neon",
    "t/unused-nested.neon",
    "t/unused-parameter.neon",
    "t/unused-repeat.neon",
    "t/unused-return.neon",
    "t/unused-scope.neon",
    "t/utf8-invalid.neon",
    "t/valid-pointer.neon",
    "t/value-copy.neon",
    "t/value-index.neon",
    "t/value-method2.neon",
    "t/value-method3.neon",
    "t/value-method.neon",
    "t/var-declaration2.neon",
    "t/var-declaration.neon",
    "t/var.neon",
    "t/var-reset.neon",
    "t/while.neon",
    "t/while-valid.neon",
    "t/win32-test.neon",
    "t/xml-test.neon",

    "lib/compress/t/compress-test.neon",            # module
    "lib/extsample/t/extsample-test.neon",          # module
    "lib/hash/t/hash-test.neon",                    # module
    "lib/http/t/http-test.neon",                    # module
    "lib/regex/t/regex-test.neon",                  # module
    "lib/sodium/t/sodium-test.neon",                # module
    "lib/zeromq/t/zeromq-test.neon",                # module
]

fullname = sys.argv[1]
path, name = os.path.split(fullname)

if fullname.replace("\\", "/") in ExcludeTests:
    sys.exit(99)

subprocess.check_call([os.path.join("bin", "neonc"), "-q", fullname])
subprocess.check_call(["exec/rsnex/rsnex", fullname + "x"])
