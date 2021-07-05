until $(curl --output /dev/null --head --fail http://localhost:8091); do
            printf '.'
            sleep 5
          done