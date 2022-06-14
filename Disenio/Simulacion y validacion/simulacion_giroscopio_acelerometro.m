clc
clear all
close all

dt = 1/60;

for i = 1 : 3
    numero = num2str(i);
    nombre = "q" + numero + "_vel.mat";
    load(nombre)
end

for i = 1 : 3
    numero = num2str(i);
    nombre = "q" + numero + "_tra.mat";
    load(nombre)
end

q1_g_sin_ruido = q1_vel;
q2_g_sin_ruido = q2_vel;
q3_g_sin_ruido = q3_vel;

q1_a_sin_ruido = q1_tra;
q2_a_sin_ruido = q2_tra;
q3_a_sin_ruido = q3_tra;

q1_g = zeros(1, 121);
q2_g = zeros(1, 121);
q3_g = zeros(1, 121);

% em = 0.0392*3;
em = 0.1;
for i = 1:121
    q1_g(i) = q1_vel(i) + normrnd(0, em);
    q2_g(i) = q2_vel(i) + normrnd(0, em);
    q3_g(i) = q3_vel(i) + normrnd(0, em);
end

q1_a = zeros(1, 121);
q2_a = zeros(1, 121);
q3_a = zeros(1, 121);

% egyr = 0.0025*2;
ep = 0.01;
for i = 1:120
    q1_a(i+1) = q1_a(i) + q1_g(i+1)*dt +normrnd(0, ep);
    q2_a(i+1) = q2_a(i) + q2_g(i+1)*dt +normrnd(0, ep);
    q3_a(i+1) = q3_a(i) + q3_g(i+1)*dt +normrnd(0, ep);
end

subplot(2, 3, 1)
hold on
plot(q1_g_sin_ruido, "r", "LineWidth", 2)
plot(q1_g, "b", "LineWidth", 0.7)

subplot(2, 3, 2)
hold on
plot(q2_g_sin_ruido, "r", "LineWidth", 2)
plot(q2_g, "b", "LineWidth", 0.7)

subplot(2, 3, 3)
hold on
plot(q3_g_sin_ruido, "r", "LineWidth", 2)
plot(q3_g, "b", "LineWidth", 0.7)

subplot(2, 3, 4)
hold on 
plot(q1_a_sin_ruido, "r", "LineWidth", 2)
plot(q1_a, "b", "Linewidth", 0.7)

subplot(2, 3, 5)
hold on 
plot(q2_a_sin_ruido, "r", "LineWidth", 2)
plot(q2_a, "b", "Linewidth", 0.7)

subplot(2, 3, 6)
hold on 
plot(q3_a_sin_ruido, "r", "LineWidth", 2)
plot(q3_a, "b", "Linewidth", 0.7)

close all

q2_a = q2_a + 1.0472;
q3_a = q3_a + 1.5708;

subplot(2, 3, 1)
hold on
plot(q1_g_sin_ruido, "r", "LineWidth", 2)
plot(q1_g, "b", "LineWidth", 0.7)

subplot(2, 3, 2)
hold on
plot(q2_g_sin_ruido, "r", "LineWidth", 2)
plot(q2_g, "b", "LineWidth", 0.7)

subplot(2, 3, 3)
hold on
plot(q3_g_sin_ruido, "r", "LineWidth", 2)
plot(q3_g, "b", "LineWidth", 0.7)

subplot(2, 3, 4)
hold on 
plot(q1_a_sin_ruido, "r", "LineWidth", 2)
plot(q1_a, "b", "Linewidth", 0.7)

subplot(2, 3, 5)
hold on 
plot(q2_a_sin_ruido, "r", "LineWidth", 2)
plot(q2_a, "b", "Linewidth", 0.7)

subplot(2, 3, 6)
hold on 
plot(q3_a_sin_ruido, "r", "LineWidth", 2)
plot(q3_a, "b", "Linewidth", 0.7)

for i = 1 : 3
    numero = num2str(i);
    nombre = "q" + numero + "_giroscopio.mat";
    variable = "q" + numero + "_g";
    save(nombre, variable)
end

for i = 1 : 3
    numero = num2str(i);
    nombre = "q" + numero + "_acelerometro.mat";
    variable = "q" + numero + "_a";
    save(nombre, variable)
end