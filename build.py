#!/usr/bin/python
import sys, subprocess as sp

if sp.call(['dotnet', 'restore']) is not 0:
    sys.exit(1)

args = sys.argv

args[0] = 'msbuild'

if sp.call(args) is not 0:
    sys.exit(2)

sys.exit(0)
