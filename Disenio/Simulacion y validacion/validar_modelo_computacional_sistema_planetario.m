clc 
clear all
close all

Sol_masa = 1988500 * 10^24;
Tierra_distancia = 149.6 * 1000 * 10^6;
Tierra_velocidad = 29.8 *1000;
G = 6.672 * 10^-11;

muestras = 6000;

tiempo = 31536000;
T = tiempo / muestras;

x = Tierra_distancia * sin(pi/4);
xp = Tierra_velocidad * - sin(pi/4);

y = Tierra_distancia * sin(pi/4);
yp = Tierra_velocidad * - cos(3*pi/4);

G_Sol_masa = G * Sol_masa;

px = zeros(1, muestras);
py = zeros(1, muestras);

vx = zeros(1, muestras);
vy = zeros(1, muestras);

for i = 1:muestras
    
    distancia_2 = x^2 + y^2;
    distancia = sqrt(distancia_2);
    distancia_3 = distancia * distancia_2;
    
    ax = -x * ( G_Sol_masa ) / distancia_3;
    ay = -y * ( G_Sol_masa ) / distancia_3;
    
    x = x + xp*T;
    xp = xp + ax*T;
    px(i) = x;
    vx(i) = xp;
    
    y = y + yp*T;
    yp = yp + ay*T;
    py(i) = y;
    vy(i) = yp;
end

subplot(1, 2, 1)
plot(px, py)

subplot(1, 2, 2)
plot(vx, vy)
