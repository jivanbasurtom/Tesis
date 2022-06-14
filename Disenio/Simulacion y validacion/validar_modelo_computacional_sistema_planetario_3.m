clc 
clear all
close all

Sol_masa = 1988500 * 10^24;
Tierra_distancia = 149.6 * 1000 * 10^6;
Tierra_velocidad = 29.8 *1000;
G = 6.672 * 10^-11;

muestras = 60*1000000;

tiempo = 31536000;
T = tiempo / muestras;

x = Tierra_distancia * sin(pi/4);
xp = Tierra_velocidad * - sin(pi/4);

y = Tierra_distancia * sin(pi/4);
yp = Tierra_velocidad * - cos(3*pi/4);

xd = Tierra_distancia * sin(pi/4);
xpd = Tierra_velocidad * - sin(pi/4);

yd = Tierra_distancia * sin(pi/4);
ypd = Tierra_velocidad * - cos(3*pi/4);

G_Sol_masa = G * Sol_masa;

Tn = (T^2)/2;

distancia_2 = x^2 + y^2;
distancia = sqrt(distancia_2);
distancia_3 = distancia * distancia_2;

ax = -x * ( G_Sol_masa ) / distancia_3;
ay = -y * ( G_Sol_masa ) / distancia_3;


tic
for i = 1:muestras
%     x = x + xp*T;
%     xp = xp + ax*T;
%     
%     y = y + yp*T;
%     yp = yp + ay*T;
    
    xd = xd + xpd*T + Tn*ax;
    xpd = xpd + ax*T;

    yd = yd + ypd*T + Tn*ay;
    ypd = ypd + ay*T;
end
toc
