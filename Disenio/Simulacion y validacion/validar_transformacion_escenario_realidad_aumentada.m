clc
clear all
close all

syms q1 q2 q3 tx ty tz

rotx_sim = [1, 0, 0
            0, cos(q1), -sin(q1)
            0, sin(q1), cos(q1)];
roty_sim = [cos(q2), 0, sin(q2)
            0, 1, 0
            -sin(q2), 0, cos(q2)];
rotz_sim = [cos(q3), -sin(q3), 0
            sin(q3), cos(q3), 0
            0, 0, 1];
        
tras = [tx; ty; tz];
rot = rotx_sim * roty_sim * rotz_sim;

T_RA = subs(tras,[tx, ty, tz], [1, 3, 1]);
R_RA = subs(rot, [q1, q2, q3] ,[pi/8, -pi/8, -pi/8]);

Plano_x = [-1, -1, 1, 1, -1];
Plano_y = [0, 0, 0, 0, 0];
Plano_z = [-1, 1, 1, -1, -1];
% Plano_y_2 = ones(1, 5)*0.5;

Plano_x_RA = zeros(1, 5);
Plano_y_RA = zeros(1, 5);
Plano_z_RA = zeros(1, 5);

linea_x = [0, 2];
linea_y = [0, 3];
linea_z = [0, 2];

linea_x_RA_1 = [0, 0];
linea_y_RA_1 = [0, 0];
linea_z_RA_1 = [0, 0];

distancia_1 = 0;
distancia_2 = 0;

for i = 1 : 5
    Ti = [ Plano_x(i); Plano_y(i); Plano_z(i)];
    Ti_RA = R_RA*Ti + T_RA;
    
    for j = 1 : 2
       distancia_1 = distancia_1 + vpa(sqrt((linea_x(j)-Ti_RA(1))^2 + (linea_y(j)-Ti_RA(2))^2 + (linea_z(j)-Ti_RA(3))^2));
    end
    
    Plano_x_RA(i) = Ti_RA(1);
    Plano_y_RA(i) = Ti_RA(2);
    Plano_z_RA(i) = Ti_RA(3);
end

for i = 1 : 2
    Pi = [linea_x(i); linea_y(i); linea_z(i)]; 
    R_RA_inv = inv(R_RA);
    Pi_RA_1 = R_RA_inv*(Pi-T_RA);
    
    for j = 1 : 5
        distancia_2 = distancia_2 + vpa(sqrt((Plano_x(j)-Pi_RA_1(1))^2 + (Plano_y(j)-Pi_RA_1(2))^2 + (Plano_z(j)-Pi_RA_1(3))^2));
    end
    
    linea_x_RA_1(i) = Pi_RA_1(1);
    linea_y_RA_1(i) = Pi_RA_1(2);
    linea_z_RA_1(i) = Pi_RA_1(3);
end

hold on
plot3(Plano_x, Plano_y, Plano_z,"r")
plot3(linea_x, linea_y, linea_z,"b")

plot3(Plano_x_RA, Plano_y_RA, Plano_z_RA,"b")
plot3(linea_x_RA_1, linea_y_RA_1, linea_z_RA_1,"r")
grid on
xlim([-4 4])
ylim([-4 4])
zlim([-4 4])