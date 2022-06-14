clc
clear all
close all

J = 1 * 10^-3;
D = 1 * 10^-2;

tiempo = 100;
muestras = 6000;

periodo = tiempo / muestras;

tiempo_transcurrido = 0;

theta = 0;
theta_p = 100;

v_theta = zeros(1, muestras);
v_theta_p = zeros(1, muestras);

T = -.01;
k = exp(-D*periodo/J);
for i = 1 : muestras
    
    theta = theta + theta_p*(J/D)*(1-k) + (D*periodo -J +J*k)*T/D^2;
    theta_p = k*theta_p + ((1-k)/D)*T;
    
    v_theta(i) = theta;
    v_theta_p(i) = theta_p;
    
    tiempo_transcurrido = tiempo_transcurrido + periodo;
end

subplot(1, 2, 1)
plot(v_theta)

subplot(1, 2, 2)
plot(v_theta_p)
