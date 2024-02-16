
Copied from: https://gist.github.com/ryderstorm/61943436751cb2d848202cda0ad26dd2#file-limit_docker_resource_usage_on_linux-md

# The Problem
Linux doesn't have a dedicated Docker application like Windows and OSX that allows you to easily place resource constraints on the docker daemon. So if you spin up a bunch of resource hungry containers, it'll slow your computer to a crawl and/or start a fire.
# Observe the Problem

Uses the image from https://hub.docker.com/r/polinux/stress to see the issue in practice. Run these beforehand to see just how much CPU/RAM docker can take up, and then run them making the adjustments below to see how the docker containers get limited.

You'll want to install [ctop](https://github.com/bcicen/ctop) and have it running to be able to monitor the container usage:

```shell
sudo wget https://github.com/bcicen/ctop/releases/download/v0.7.3/ctop-0.7.3-linux-amd64 -O /usr/local/bin/ctop
sudo chmod +x /usr/local/bin/ctop
```

### Run a single CPU stress

```shell
docker run -it --rm polinux/stress stress --cpu 1 -t 30
```

### Run a single RAM stress

```shell
docker run -it --rm polinux/stress stress --vm-bytes 1G --vm-keep -m 1
```

### Run multiple CPU stress containers

```
docker run -d --rm --name stress_1 polinux/stress stress --cpu 1 -t 30 && \
docker run -d --rm --name stress_2 polinux/stress stress --cpu 1 -t 30 && \
docker run -d --rm --name stress_3 polinux/stress stress --cpu 1 -t 30 && \
docker run -d --rm --name stress_4 polinux/stress stress --cpu 1 -t 30
```

### Run multiple RAM stress containers

```shell
docker run -d --rm --name stress_1 polinux/stress stress --vm-bytes 4G --vm-keep -m 1 -t 30 && \
docker run -d --rm --name stress_2 polinux/stress stress --vm-bytes 4G --vm-keep -m 1 -t 30 && \
docker run -d --rm --name stress_3 polinux/stress stress --vm-bytes 4G --vm-keep -m 1 -t 30
```

# Fix the Problem

If you want to know how/why this works, see the [References](#references) section below

## Create config files for docker daemon

Edit `/etc/docker/daemon.json` with this content:

```shell
{
  "cgroup-parent": "/docker_limit.slice"
}
```

Edit `/etc/systemd/system/docker_limit.slice` with this content:

```shell
Description=Slice that limits docker resources
Before=slices.target

[Slice]
# CPU Management
CPUAccounting=true
CPUQuota=333%

# Memory Management
MemoryAccounting=true
MemoryHigh=4G
MemoryMax=6G
```

## Enable and start docker slice

```shell
sudo systemctl daemon-reload && sudo systemctl enable docker_limit.slice && sudo systemctl start docker_limit.slice
# sudo systemctl restart docker # not necessary
```

## Restart related services
```shell
sudo systemctl daemon-reload && sudo systemctl restart docker_limit.slice
# sudo systemctl restart docker # not necessary
```

# Known Issue

I haven't figured out how to make this fix stick after a reboot yet. Probably a simple fix, i just haven't looked into it hard yet. In the meantime, running the command in [Restart related services](#restart-related-services)

---

# References
Details on resource control unit settings:
https://www.freedesktop.org/software/systemd/man/systemd.resource-control.html

Details on the Docker configuration file:
https://docs.docker.com/engine/reference/commandline/dockerd/#daemon-configuration-file

Original stackexchange answer that inspired this:
https://unix.stackexchange.com/a/550954

Related issue on docker repo:
https://github.com/moby/moby/issues/29278