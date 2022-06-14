clc 
clear all
close all

Sol_masa = 1988500 * 10^24;
Tierra_distancia = 149.6 * 1000 * 10^6;
Tierra_velocidad = 29.8 *1000;
G = 6.672 * 10^-11;

px = zeros(1, 120);
py = zeros(1, 120);

muestras = 60*100;

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

px = zeros(1, muestras);
py = zeros(1, muestras);

vx = zeros(1, muestras);
vy = zeros(1, muestras);

pxd = zeros(1, muestras);
pyd = zeros(1, muestras);

vxd = zeros(1, muestras);
vyd = zeros(1, muestras);

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
    
    xd = xd + xpd*T + ((T^2)/2)*ax;
    xpd = xpd + ax*T;
    pxd(i) = xd;
    vxd(i) = xpd;
    
    yd = yd + ypd*T + ((T^2)/2)*ay;
    ypd = ypd + ay*T;
    pyd(i) = yd;
    vyd(i) = ypd;
end

xlim([-Tierra_distancia Tierra_distancia])
ylim([-Tierra_distancia Tierra_distancia])

subplot(2, 2, 1)
plot(px, py)

subplot(2, 2, 2)
plot(vx, vy)

subplot(2, 2, 3)
plot(pxd, pyd)

subplot(2, 2, 4)
plot(vxd, vyd)

sum_E = 0;
for i = 1 : muestras
    distancia1 = sqrt( px(i)^2 + py(i)^2 );
    distancia2 = sqrt ( pxd(i)^2 + pyd(i)^2 );
    E = abs( distancia1 - distancia2 );
    prom_distancia = ( distancia1 + distancia2 )/2;
    sum_E = sum_E + E/prom_distancia;
end

Error_prom = sum_E/muestras;
Error_por = Error_prom * 100;

% prom_distancia = ( distancia1 + distancia2 )/2;
% disp( ( (MESd/muestras) / prom_distancia) * 100 )
%     
% 0.0332