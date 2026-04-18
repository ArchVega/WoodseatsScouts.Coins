#!/bin/bash
# chmod +x debuggable-chromium.sh

/snap/bin/chromium \
  --remote-debugging-port=9222 \
  http://localhost:5173/