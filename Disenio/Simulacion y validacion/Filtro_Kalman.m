clc
clear all
close all

dt = 1/60;

for i = 1 : 3
    numero = num2str(i);
    nombre = "q" + numero + "_vel.mat";
    load(nombre)
    nombre = "q" + numero + "_tra.mat";
    load(nombre)
end

for i = 1 : 3
    numero = num2str(i);
    nombre = "q" + numero + "_acelerometro.mat";
    load(nombre)
    nombre = "q" + numero + "_giroscopio.mat";
    load(nombre)
end

q1_g_c = q1_vel;
q2_g_c = q2_vel;
q3_g_c = q3_vel;
q1_a_c = q1_tra;
q2_a_c = q2_tra;
q3_a_c = q3_tra;


pos_1 = zeros(1, 121);
vel_1 = zeros(1, 121);
pos_2 = zeros(1, 121);
vel_2 = zeros(1, 121);
pos_3 = zeros(1, 121);
vel_3 = zeros(1, 121);

w = normrnd(0, 0.2, [1, 6]); %error proceso
v = normrnd(0, 0.2,[1, 6]); %error estimacion

zero = zeros(2, 2);
F = [1, 1/60; 0, 1];
F = [F, zero, zero; zero, F, zero; zero, zero, F];
H = eye(6, 6)*0.7;
X0 = zeros(6, 1);

I = eye(6);
P0 = w' * w;
R = v' * v;
R = diag(R).*I;
Q = w' * w;
Q = diag(Q).*I;

for i = 1 : 121
    w = normrnd(0, 0.1,[1, 6]);
    v = normrnd(0, 0.01,[1, 6]);
    X = F * X0 +[w(1); 0; w(3); 0; w(5); 0];
    P = F * P0 * F' + Q; %con matriz Q
%     P = F * P0 * F';
    P = diag(P).*I; %considerar seguimiento
    S = H * P * H' + R; %+R
    K = P * H' * inv(S);
    Z = [q1_a_c(i); q1_g_c(i); q2_a_c(i); q2_g_c(i); q3_a_c(i); q3_g_c(i)];
%     Z = Z + v'; %error de medicion 
    Z = H*Z + v'; %comunicación inalambrica y error de medicion
    d = Z - H * X;
    X0 = X + K * d;
    P0 = (I - K*H) * P;
    pos_1(i) = X0(1);
    vel_1(i) = X0(2);
    pos_2(i) = X0(3);
    vel_2(i) = X0(4);
    pos_3(i) = X0(5);
    vel_3(i) = X0(6);
end

med_1 = sum((vel_1 - q1_g_c).^2);
med_2 = sum((vel_2 - q2_g_c).^2);
med_3 = sum((vel_3 - q3_g_c).^2);
med_4 = sum((pos_1 - q1_a_c(1:121)).^2);
med_5 = sum((pos_2 - q2_a_c(1:121)).^2);
med_6 = sum((pos_3 - q3_a_c(1:121)).^2);
rendimiento = (med_1 + med_2 + med_3 + med_4 + med_5 + med_6)/6;

subplot(2, 3, 1)
plot(pos_1)
subplot(2, 3, 2)
plot(pos_2)
subplot(2, 3, 3)
plot(pos_3)


subplot(2, 3, 4)
plot(vel_1)
subplot(2, 3, 5)
plot(vel_2)
subplot(2, 3, 6)
plot(vel_3)

disp(rendimiento)


