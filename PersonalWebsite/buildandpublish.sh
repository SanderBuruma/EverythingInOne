sudo docker login
sudo docker image prune --all --force
sudo docker build -t sanderburuma/personal-website:latest .
sudo docker push sanderburuma/personal-website