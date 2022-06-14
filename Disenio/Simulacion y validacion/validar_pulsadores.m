clc
clear all
close all

% parametros 1:velocidad 2:direccion 3:masa

param_cambios=[0, 0, 0];
param_finales=[0, 0, 0];

clic_izquierdo = 0;
clic_derecho = 0;

clic_izquierdo_ant = 0;
clic_derecho_ant = 0;

estado = 4;

cambiar_parametro = 0;
guardar_parametro = 0;

reiniciar_parametro = 0;
regresar_parametro = 0;

modificar_parametro = 0;

tipo_parametro = 1;

tiempo = 30;
estado_ = zeros(1, tiempo); 
estado_0 = zeros(1, tiempo);
estado_1 = zeros(1, tiempo);
estado_2 = zeros(1, tiempo);
estado_3 = zeros(1, tiempo);
estado_4 = zeros(1, tiempo);
clic_d = zeros(1, tiempo);
clic_i = zeros(1, tiempo);

contador = 0;

for i = 1:tiempo
    estado_(i) = estado;
    switch estado
        case 0
            cambiar_parametro = 1;
        case 1 
            guardar_parametro = 1;
        case 2
            reiniciar_parametro = 1;
        case 3
            regresar_parametro = 1;
        case 4
            modificar_parametro = 1;
        otherwise
            disp("salir")
            break
    end
   
    if(i == 2 || i == 7 || i == 21 || i == 24)
        clic_izquierdo = 1;
    else
        clic_izquierdo = 0;
    end
    
    if(i == 12 || i == 17 || i == 18)
        clic_derecho = 1;
    else
        clic_derecho = 0;
    end
    
    
    estado_0(i) = cambiar_parametro;
    estado_1(i) = guardar_parametro;
    estado_2(i) = reiniciar_parametro;
    estado_3(i) = regresar_parametro;
    estado_4(i) = modificar_parametro;
    clic_d(i) = clic_derecho;
    clic_i(i) = clic_izquierdo;
    
    if(cambiar_parametro == 1)
        tipo_parametro = tipo_parametro + 1;
        estado = 4;
        cambiar_parametro = 0;
    end
    
    if(guardar_parametro == 1)
        param_finales(tipo_parametro) = param_cambios(tipo_parametro);
        if(tipo_parametro == 3)
            estado = -1;
        else
            estado = 0;
        end
        guardar_parametro = 0;
    end
    
    if(reiniciar_parametro == 1)
        param_cambios(tipo_parametro) = param_finales(tipo_parametro);
        estado = 4;
        reiniciar_parametro = 0;
    end
    
    if(regresar_parametro == 1)
        tipo_parametro = tipo_parametro -1;
        estado = 4;
        if(tipo_parametro == 0)
            estado = -1;
        end
        regresar_parametro = 0;
    end
    
    if(modificar_parametro == 1)
        param_cambios(tipo_parametro) = param_cambios(tipo_parametro) + 1;
    end
    
    if(clic_izquierdo == 0 && clic_izquierdo_ant == 1)
        estado = 1;
    end
    clic_izquierdo_ant = clic_izquierdo;
    
    if(clic_derecho == 1)
        contador = contador + 1; 
    end
    
    if(clic_derecho == 0 && clic_derecho_ant == 1)
        estado = 2;
        if(contador > 1)
            estado = 3;
            contador = 0;
        end
    end
    clic_derecho_ant = clic_derecho;
    
    vec = [i, param_cambios, param_finales];
    disp(vec)
end

t = linspace(1, tiempo, tiempo); 


% subplot(1,2,1)
% xlim([0 30])
% plot(t, clic_i, "ro", "MarkerSize", 7, "MarkerEdgeColor", "r", "MarkerFaceColor", [1,0,0])
% grid on
% title("clic izquierdo")
% 
% hold on
% subplot(1,2,2)
% xlim([0 30])
% plot(t, clic_d, "ro", "MarkerSize", 7, "MarkerEdgeColor", "r", "MarkerFaceColor", [1,0,0])
% grid on
% title("clic derecho")

% hold on
% subplot(2,2,1)
% xlim([0 30])
% plot(t, estado_0, "ro", "MarkerSize", 7, "MarkerEdgeColor", "r", "MarkerFaceColor", [1,0,0])
% grid on
% title("cambiar parametro")
% 
% hold on
% subplot(2,2,2)
% xlim([0 30])
% plot(t, estado_1, "ro", "MarkerSize", 7, "MarkerEdgeColor", "r", "MarkerFaceColor", [1,0,0])
% grid on
% title("guardar parametro")
% 
% hold on
% subplot(2,2,3)
% xlim([0 30])
% plot(t, estado_2, "ro", "MarkerSize", 7, "MarkerEdgeColor", "r", "MarkerFaceColor", [1,0,0])
% grid on
% title("reiniciar parametro")
% 
% hold on
% subplot(2,2,4)
% xlim([0 30])
% plot(t, estado_3, "ro", "MarkerSize", 7, "MarkerEdgeColor", "r", "MarkerFaceColor", [1,0,0])
% grid on
% title("regresar parametro")
% 
% hold on
% subplot(2,4,7)
% xlim([0 30])
% plot(t, estado_4, "ro")
% title("modificar parametro")


xlim([0 30])
ylim([-1 4])
plot(t, estado_, "ro", "MarkerSize", 7, "MarkerEdgeColor", "r", "MarkerFaceColor", [1,0,0])
grid on
title("estado")