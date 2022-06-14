clc
clear all
close all

F = [1,1, 0.5
     0, 1, 1
     0, 0, 1];
H = eye(3, 3);

X00 = [100; 20 ; 3];  %observacion inicial

X10 = F*X00;

P = [10, 2, 0.1];
P00 = P'*P;
P10 = F*P00*F';
P10 = diag(P10).*eye(3, 3);

vk = [15, 4, 0.2];
R = vk' *vk;
R = diag(R).*eye(3, 3);

S1 = H*P10*H' + R;
K1 = P10*H'*inv(S1);

Z = [130, 23, 3];
delta1 = Z' - H*X10;
X11 = X10 + K1*delta1;
P11 = (eye(3, 3) - K1*H)*P10;