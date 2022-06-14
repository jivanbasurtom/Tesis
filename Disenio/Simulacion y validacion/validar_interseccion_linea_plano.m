clc
clear all
close all

syms q1

rotz_sim = [cos(q1), -sin(q1), 0
            sin(q1), cos(q1), 0
            0, 0, 1];

Plano_x = [-1.5, -1.5, 1.5, 1.5, -1.5];
Plano_y = [0, 0, 0, 0, 0];
Plano_z = [-1, 1, 1, -1, -1];

linea_x = [0, 4];  
linea_y = [-2, 1];
linea_z = [0, 0];

T = [0; 2; 0];

angulos = [0, pi/8, 2*pi/8, 3*pi/8, 4*pi/8, 5*pi/8, 6*pi/8, 7*pi/8, 8*pi/8 ];

P0 = [0, 0, 0];
P1 = [0, 0, 1];
P2 = [1.5, 0, 0];
P10 = P0 - P1;
P20 = P0 - P2;
hx = -P20(1);
hz = -P10(3);

Pa = [linea_x(1); linea_y(1); linea_z(1)];
Pb0 = [linea_x(2); linea_y(2); linea_z(2)];
Pa0 = P0' - Pa;

Puntos_x = zeros(1, 8);
Puntos_y = zeros(1, 8);
Puntos_z = zeros(1, 8);

lineas = zeros(3, 18);

for i = 1:9
    Rotz = subs(rotz_sim, q1, angulos(i));
    Pb = double(vpa(Rotz*(Pb0 + T))- T);
    Pab = Pa - Pb;
    
    lineas(:, 2*(i-1)+1) = Pa;
    lineas(:, 2*i) = Pb;
    
    v = (Pab(1)*Pa0(2) - Pab(2)*Pa0(1))/(-hx*Pab(2));
    u = (Pab(3)*Pa0(2) - Pab(2)*Pa0(3))/(-hz*Pab(2));

    if (-1 <= u &&  u <=1 && -1 <= v && v <= 1)
        P = P0 + P10*u + P20*v;
        Puntos_x(i) = P(1);
        Puntos_y(i) = P(2);
        Puntos_z(i) = P(3);
    else
        Puntos_x(i) = inf;
        Puntos_y(i) = inf;
        Puntos_z(i) = inf;
    end
end
hold on
plot3(Plano_x, Plano_y, Plano_z)

for i = 1 : 9
    plot3(lineas(1, (2*(i-1))+1:2*i), lineas(2, (2*(i-1))+1:2*i), lineas(3, (2*(i-1))+1:2*i))
end
plot3(Puntos_x, Puntos_y, Puntos_z, "r*")

grid on
xlim([-4, 4])
ylim([-4, 4])
zlim([-4, 4])