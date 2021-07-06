#!/usr/bin/env python3

import os
import subprocess
import sys

fullname = sys.argv[1]
path, name = os.path.split(fullname)

subprocess.check_call(["exec/gonex/gonex", fullname + "x"] + sys.argv[2:])
