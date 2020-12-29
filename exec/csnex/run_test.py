#!/usr/bin/env python3

import os
import subprocess
import sys

csnex = os.path.join("exec", "csnex", "csnex")

i = 1
while i < len(sys.argv):
    if sys.argv[i] == "--csnex":
        i += 1
        csnex = sys.argv[i]
    else:
        break
    i += 1

fullname = sys.argv[i]

subprocess.check_call([csnex, fullname + "x"] + sys.argv[i+1:])
