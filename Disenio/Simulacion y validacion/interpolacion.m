clc
clear all
close all

p0 = 0;
p1 = pi/2;
v0 = 0;
vf = 0;
vt0 = 0;
vtf = 1;
dt = 0.01;

vec = int3(p0, p1, v0, vf, vt0, vtf, dt);
plot(vec)