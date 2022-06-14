clc
clear all
close all

D = 1 * 10^-1;
m = 1;
g = -9.81;

A = [0, 1 ; 0, -D/m];
B = [0 ; 1];

y = 0;
yp = 100;

muestras = 900;
tiempo = 15;
dt = tiempo / muestras;

v_y = zeros(1, muestras);
v_yp = zeros(1, muestras);

for i = 1 : muestras 

    y = y + yp*dt;
    yp = yp + ( ((-D/m)*yp) + g)*dt;

    v_y(i) = y;
    v_yp(i) = yp;
end

subplot(1, 2, 1)
plot(v_y)

subplot(1, 2, 2)
plot(v_yp)