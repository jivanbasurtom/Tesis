clc
clear all
close all

syms s x xp ax t dt

A = [0, 1 ; 0, 0];
B = [0 ; 1];
I = eye(2);
Mat_exp = ilaplace(inv(s*I - A));
Bd = int(Mat_exp, 0, dt)*B;
Ad = Mat_exp;

disp(Ad)
disp(Bd)

% A = subs(Ad, 8760);
% B = subs(Bd, 8760);

syms J D s dt
% J = 1 * 10^-3;
% D = 1 * 10^-2;
% syms c1 c2
% c1 = -D/J;
% c2 = 1/J;
A = [0, 1 ; 0, -D/J];
B = [0 ; 1/J];
I = eye(2);
Mat_exp = ilaplace(inv(s*I - A));
Bd = int(Mat_exp, 0, dt)*B;
Ad = Mat_exp;

disp(Ad)
disp(Bd)

syms D m
% D = 1 * 10^-1;
% m = 1;
A = [0, 1 ; 0, -D/m];
B = [0 ; 1];
I = eye(2);
Mat_exp = ilaplace(inv(s*I - A));
Bd = int(Mat_exp, 0, dt)*B;
Ad = Mat_exp;

% A = subs(Ad,[D, m t], [.01, .1, 1] );

disp(Ad)
disp(Bd)