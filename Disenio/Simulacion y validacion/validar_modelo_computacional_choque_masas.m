clc
clear all
close all

VA1 = 0;
choque_V2_A = 0;
choque_V2_B = 0;
e = 0.2;

PAy = 0;
PBy = 0.02;
distancia = 0.1;

PAy_vel_1 = 30;
PAy_Fuerza = 1000;
Masa_A = 10;

PBy_vel_2 = 0;
Masa_B = .1;

contacto_AB = 0;
contacto_AB_anterior = 0;

sum_Fuerza = 0;
dt = 0.0001;

v_PA = zeros(1, 2000);
v_PB = zeros(1, 2000);

B_mover = false;

for i = 1 : 2000
    if(PAy >= PBy)
       contacto_AB = 1; 
    end
    
    if(contacto_AB == 1 && contacto_AB_anterior == 0)
        VA1 = PAy_vel_1;
        B_mover = true;
    end
    
    if(PBy >= distancia)
        B_mover = false;
    end
  
    if(B_mover == true)
        PBy = PAy;
        sum_Fuerza = sum_Fuerza + PAy_Fuerza*dt;
    end
    
    if(PAy >= distancia)
       PBy = PBy + dt*3; 
    end
    
    if(PAy <= distancia)
        PAy = PAy + dt;
    end
    
    v_PA(i) = PAy;
    v_PB(i) = PBy;
    
    contacto_AB_anterior = contacto_AB;
    PAy_vel_1 = PAy_vel_1 + 100*dt;
end

R1 = Masa_A*VA1 + sum_Fuerza;
R2 = e * VA1;
Detg = - Masa_A - Masa_B;
DetVB2 = R2 * Masa_B - R1;
DetVA2 = - (R1 + Masa_A*R2);
VB2 = DetVB2 / Detg;
VA2 = DetVA2 / Detg;

subplot(1, 2, 1)
plot(v_PA)
ylim([0 0.3])

subplot(1, 2, 2)
plot(v_PB)
ylim([0 0.3])

% choque de masas
% e = 0.5;
% Masa_A = 10;
% Masa_B = .1;
% 
% VA1 = 20;
% 
% sum_Fuerza = 100;
% 
% R1 = Masa_A*VA1 + sum_Fuerza;
% R2 = e * VA1;
% Detg = - Masa_A - Masa_B;
% DetVB2 = R2 * Masa_B - R1;
% DetVA2 = - (R1 + Masa_A*R2);
% VB2 = DetVB2 / Detg;
% VA2 = DetVA2 / Detg;