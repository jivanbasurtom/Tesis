clc
clear all
close all

syms q1 q2 q3 

R01 = [cos(q1), -sin(q1), 0; sin(q1), cos(q1), 0; 0, 0, 1];
R12 = [cos(q2), -sin(q2), 0; 0, 0, 1; -sin(q2), -cos(q2), 0];
R23 = [0, 0, 1; -cos(q3), sin(q3), 0; -sin(q3), -cos(q3), 0];

angulos_q1 = [0, pi/4, pi/2];
angulos_q2 = [pi/3, pi/6, pi/3];
angulos_q3 = [pi/2, pi/4, 0];

R_sym = R01*R12*R23;

% R_1 = double(subs( R_sym, [q1, q2, q3], [angulos_q(1), angulos_q(1), angulos_q(1)] ));
% R_2 = double(subs( R_sym, [q1, q2, q3], [angulos_q(2), angulos_q(2), angulos_q(2)] ));
% R_3 = double(subs( R_sym, [q1, q2, q3], [angulos_q(3), angulos_q(3), angulos_q(3)] ));

% r = R_3;

q1v = zeros(1, 3);
q2v = zeros(1, 3);
q3v = zeros(1, 3);

dt = 1/60;

for i = 1 : 3
    r = double(subs( R_sym, [q1, q2, q3], [angulos_q1(i), angulos_q2(i), angulos_q3(i)] ));
    
    q2v(i) = asin(-r(3,3));
    if(r(1,3) == 0)
        q1v(i) = pi/2;
    else
        q1v(i) = atan(r(2,3)/r(1,3));
    end

    if(r(3,1) == 0)
        q3v(i) = pi/2;
    else
        q3v(i) = atan(-r(3,2)/r(3,1));
    end

end

q1_tra = [];
q2_tra = [];
q3_tra = [];

for i = 1 : 2
    q1_tra = [q1_tra, int3(q1v(i), q1v(i+1), 0, 0, 0, 1, dt) ];
    q2_tra = [q2_tra, int3(q2v(i), q2v(i+1), 0, 0, 0, 1, dt) ];
    q3_tra = [q3_tra, int3(q3v(i), q3v(i+1), 0, 0, 0, 1, dt) ];
end


L = length(q1_tra) - 1;
q1_vel = zeros(1, L);
q2_vel = zeros(1, L);
q3_vel = zeros(1, L);

for i = 1 : L
    q1_vel(i) = (q1_tra(i+1) - q1_tra(i))/dt;
    q2_vel(i) = (q2_tra(i+1) - q2_tra(i))/dt;
    q3_vel(i) = (q3_tra(i+1) - q3_tra(i))/dt;
end
% 
% q1_vel = q1_vel + random('norm', -.1, .1, 1, 201);
% q2_vel = q2_vel + random('norm', -.1, .1, 1, 201);
% q3_vel = q3_vel + random('norm', -.1, .1, 1, 201);
% 
% q1_tra(1) = 0;
% for i = 1 : 201
%     q1_tra(i+1) = (q1_vel(i)*dt) + q1_tra(i);
% end
% subplot(2, 3, 1)
% plot(q1_tra)

subplot(2, 3, 1)
plot(q1_tra)

subplot(2, 3, 2)
plot(q2_tra)

subplot(2, 3, 3)
plot(q3_tra)

subplot(2, 3, 4)
plot(q1_vel)

subplot(2, 3, 5)
plot(q2_vel)

subplot(2, 3, 6)
plot(q3_vel)

for i = 1 : 3
    numero = num2str(i);
    nombre = "q" + numero + "_vel.mat";
    variable = "q" + numero + "_vel";
    save(nombre, variable)
end

for i = 1 : 3
    numero = num2str(i);
    nombre = "q" + numero + "_tra.mat";
    variable = "q" + numero + "_tra";
    save(nombre, variable)
end
