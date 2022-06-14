// CONFIG1H
#pragma config OSC = INTIO1     // Oscillator Selection bits (Internal RC oscillator, CLKO function on RA6 and port function on RA7)
#pragma config FSCM = OFF       // Fail-Safe Clock Monitor Enable bit (Fail-Safe Clock Monitor disabled)
#pragma config IESO = OFF       // Internal/External Switchover bit (Internal/External Switchover mode disabled)

// CONFIG2L
#pragma config PWRT = OFF       // Power-up Timer enable bit (PWRT disabled)
#pragma config BOR = OFF        // Brown-out Reset enable bit (Brown-out Reset disabled)
#pragma config BORV = 20        // Brown-out Reset Voltage bits (VBOR set to 2.0V)

// CONFIG2H
#pragma config WDT = OFF        // Watchdog Timer Enable bit (WDT disabled (control is placed on the SWDTEN bit))
#pragma config WDTPS = 1        // Watchdog Timer Postscale Select bits (1:1)

// CONFIG3H
#pragma config CCP2MX = OFF     // CCP2 MUX bit (CCP2 input/output is multiplexed with RB3)
#pragma config PBAD = DIG       // PORTB A/D Enable bit (PORTB<4:0> pins are configured as digital I/O on Reset)
#pragma config MCLRE = OFF      // MCLR Pin Enable bit (MCLR disabled; RE3 input is enabled in 40-pin devices only (PIC18F4X20))

// CONFIG4L
#pragma config STVR = OFF       // Stack Full/Underflow Reset Enable bit (Stack full/underflow will not cause Reset)
#pragma config LVP = OFF        // Single-Supply ICSP Enable bit (Single-Supply ICSP disabled)

// CONFIG5L
#pragma config CP0 = OFF        // Code Protection bit (Block 0 (000200-0007FFh) not code-protected)
#pragma config CP1 = OFF        // Code Protection bit (Block 1 (000800-000FFFh) not code-protected)

// CONFIG5H
#pragma config CPB = OFF        // Boot Block Code Protection bit (Boot block (000000-0001FFh) is not code-protected)
#pragma config CPD = OFF        // Data EEPROM Code Protection bit (Data EEPROM is not code-protected)

// CONFIG6L
#pragma config WRT0 = OFF       // Write Protection bit (Block 0 (000200-0007FFh) not write-protected)
#pragma config WRT1 = OFF       // Write Protection bit (Block 1 (000800-000FFFh) not write-protected)

// CONFIG6H
#pragma config WRTC = OFF       // Configuration Register Write Protection bit (Configuration registers (300000-3000FFh) are not write-protected)
#pragma config WRTB = OFF       // Boot Block Write Protection bit (Boot block (000000-0001FFh) is not write-protected)
#pragma config WRTD = OFF       // Data EEPROM Write Protection bit (Data EEPROM is not write-protected)

// CONFIG7L
#pragma config EBTR0 = OFF      // Table Read Protection bit (Block 0 (000200-0007FFh) not protected from table reads executed in other blocks)
#pragma config EBTR1 = OFF      // Table Read Protection bit (Block 1 (000800-000FFFh) not protected from table reads executed in other blocks)

// CONFIG7H
#pragma config EBTRB = OFF      // Boot Block Table Read Protection bit (Boot block (000000-0001FFh) is not protected from table reads executed in other blocks)

// Use project enums instead of #define for ON and OFF.
#include <xc.h>

//PIC
#define frecuencia 8000000
#define clic_izquierdo PORTAbits.RA1
#define clic_derecho PORTAbits.RA2
void Pausa(unsigned short milisegundos);
void Milisegundo(void);
void PIC_Configuracion(void);
void Leer_Pulsadores(void);

//I2C
#define I2C_BoudRate 100000 //100kHz
void I2C_Esperar(void);
void I2C_Empezar(unsigned char Direccion);
void I2C_Inicializar(void);
unsigned char I2C_Escribir(unsigned char Data);
void I2C_Detener(void);
void I2C_Reiniciar(void);
void I2C_ACK(void);
void I2C_NACK(void);
unsigned char I2C_Leer(unsigned char ACK_NACK);

//MPU
#define MPU6050_Direccion          0xD0
#define Velocidad_Muestreo         0x19 //SMPLRT_DIV
#define Reloj_Giroscopio_X         0x6B //PWR_MGMT_1
#define Configurar                 0x1A //config
#define Configurar_Acelerometro    0x1C //ACCEL_CONFIG
#define Configurar_Giroscopio      0x1B //GYRO_CONFIG
#define Habilitar_Interrupciones   0x38 //INT_ENABLE
#define Acelerometro_salida_X_Alto 0x3B //ACCEL_XOUT_H

void MPU6050_Inicializar(void);
void MPU6050_Leer(void);
int Ax, Ay, Az, T, Gx, Gy, Gz;
unsigned char Data_recibida;

//conversion
char encriptar[] = {'0', '1', '2', '3', '4', '5', '6', '7',
                    '8', '9', ':', ';', 'A', 'B', 'C', 'D',
                    'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
                    'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                    'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b',
                    'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
                    'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
                    's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
char mensaje[15];
int posicion;
int valor_normalizado;
void Encriptar(int *valor);
int valor_maximo;

//Bluetooth
#define BT_BoudRate 9600
void BT_Inicializar(void);
void BT_Cargar_Caracter(char caracter);
void BT_Cargar_Cadena_Texto(char *cadena);
void BT_Transmision(char fin_mensaje);
void BT_Referencia(char ref1, char ref2);
unsigned char BT_Leer_Caracter(void);

//funcion principal
void main(void) {
    PIC_Configuracion();
    
    unsigned char valor_recibido = 0;
    unsigned int contador = 0;
    int led = 0;
    
    BT_Inicializar();
    BT_Referencia('z', 'z');
    MPU6050_Inicializar();
    
    while(1)
    {
        valor_recibido = BT_Leer_Caracter();
        if(valor_recibido != 0)
        {
            MPU6050_Leer();
            Leer_Pulsadores();
            BT_Cargar_Cadena_Texto(mensaje);
            BT_Transmision('\n');
            contador += 2;
        }
        contador++;
        
        if(contador > 50)
        {
            PORTAbits.RA0 = (led == 0) ? 1 : 0;
            led = ~led;
            contador = 0;
        }
        Pausa(4);
    }
    return;
}

//PIC
void Pausa(unsigned short milisegundos)
{
    unsigned short milisegundo_actual;
    for(milisegundo_actual = 0; milisegundo_actual < milisegundos; milisegundo_actual++)
    {
        Milisegundo();
    }
}

void Milisegundo(void)
{
    TMR0 = 0x06;
    TMR0ON = 1;
    while(!TMR0IF);
    TMR0IF = 0;
    TMR0ON = 0;
}

void PIC_Configuracion()
{
    //Timer 0: T0CON
    T08BIT = 1;
    T0CS = 0;
    PSA = 0;
    T0PS2 = 0;
    T0PS1 = 1;
    T0PS0 = 0; //1:8
    
    //oscilador: OSCCON
    IRCF2 = 1;
    IRCF1 = 1;
    IRCF0 = 1; // 8Mhz
    SCS1 = 1; //oscilador interno
    
    //Convertidor analogico: ADCON0
    ADON = 0;
    
    //ADCON1
    PCFG3 = 1;
    PCFG2 = 1;
    PCFG1 = 1;
    PCFG0 = 1;
    
    //entradas/salidas
    TRISA0 = 0; //led
    TRISA1 = 1; //clic izquierdo
    TRISA2 = 1; //clic derecho
}

void Leer_Pulsadores(void)
{
    if( (clic_izquierdo == 1) && (clic_derecho == 1) )
    {
        mensaje[12] = '3';
    }
    else if( (clic_izquierdo == 0) && (clic_derecho == 1) )
    {
        mensaje[12] = '2';
    }
    else if( (clic_izquierdo == 1) && (clic_derecho == 0) )
    {
        mensaje[12] = '1';
    }
    else if( (clic_izquierdo == 0) && (clic_derecho == 0) )
    {
        mensaje[12] = '0';
    }
}

//I2C
void I2C_Inicializar(void)
{
    //SSPCON1
    SSPEN = 1;
    SSPM3 = 1;
    SSPM2 = 0;
    SSPM1 = 0;
    SSPM0 = 0;
    SSPSTAT = 0x00;
    SSPADD = ( (frecuencia/4) / I2C_BoudRate ) - 1;
    PORTC = 0;
    TRISC3 = 1; //SCL
    TRISC4 = 1; //SDA
}

void I2C_Esperar(void)
{
    while((SSPSTAT & 0x04 || (SSPCON2 & 0x1F)));
}

void I2C_Empezar(unsigned char Direccion)
{
    I2C_Esperar();
    SEN = 1;
    I2C_Escribir(Direccion);
}

unsigned char I2C_Escribir(unsigned char Data)
{
    I2C_Esperar();
    SSPBUF = Data;
    while(!SSPIF);
    SSPIF = 0;
    return ACKSTAT;
}

void I2C_Detener(void)
{
    I2C_Esperar();
    PEN = 1;
}

void I2C_Reiniciar(void)
{
    I2C_Esperar();
    RSEN = 1;
}

void I2C_ACK(void)
{
    ACKDT = 0;
    ACKEN = 1;
    while(ACKEN);
}

void I2C_NACK(void)
{
    ACKDT = 1;
    ACKEN = 1;
    while(ACKEN);
}

unsigned char I2C_Leer(unsigned char ACK_NACK)
{
    RCEN = 1;
    while(!BF);
    Data_recibida = SSPBUF;
    (ACK_NACK == 0) ? I2C_ACK() : I2C_NACK();
    while(!SSPIF);
    SSPIF = 0;
    return Data_recibida;
}

//MPU6050
void MPU6050_Inicializar(void)
{
    Pausa(150);
    I2C_Inicializar();

    I2C_Empezar(MPU6050_Direccion);
    I2C_Escribir(Velocidad_Muestreo);
    I2C_Escribir(0x07);
    I2C_Detener();

    I2C_Empezar(MPU6050_Direccion);
    I2C_Escribir(Reloj_Giroscopio_X);
    I2C_Escribir(0x01); 
    I2C_Detener();

    I2C_Empezar(MPU6050_Direccion);
    I2C_Escribir(Configurar);
    I2C_Escribir(0x00); 
    I2C_Detener();

    I2C_Empezar(MPU6050_Direccion);
    I2C_Escribir(Configurar_Acelerometro);
    I2C_Escribir(0x00); 
    I2C_Detener();

    I2C_Empezar(MPU6050_Direccion);
    I2C_Escribir(Configurar_Giroscopio);
    I2C_Escribir(0x08);
    I2C_Detener();

    I2C_Empezar(MPU6050_Direccion);
    I2C_Escribir(Habilitar_Interrupciones);
    I2C_Escribir(0x01); 
    I2C_Detener();
}

void MPU6050_Leer()
{
    posicion = 0;
    I2C_Empezar(MPU6050_Direccion);
    I2C_Escribir(Acelerometro_salida_X_Alto);
    I2C_Detener();
    
    I2C_Empezar(MPU6050_Direccion | 0x01); //realizar lectura de datos
    I2C_Leer(0); //Dummy, no quitar
    Ax = ((int)I2C_Leer(0)<<8) | (int)I2C_Leer(0);
    Ay = ((int)I2C_Leer(0)<<8) | (int)I2C_Leer(0);
    Az = ((int)I2C_Leer(0)<<8) | (int)I2C_Leer(0);
    T  = ((int)I2C_Leer(0)<<8) | (int)I2C_Leer(0);
    Gx = ((int)I2C_Leer(0)<<8) | (int)I2C_Leer(0);
    Gy = ((int)I2C_Leer(0)<<8) | (int)I2C_Leer(0);
    Gz = ((int)I2C_Leer(0)<<8) | (int)I2C_Leer(1);
    I2C_Detener();

    Encriptar(&Ax);
    Encriptar(&Ay);
    Encriptar(&Az);
    Encriptar(&Gx);
    Encriptar(&Gy);
    Encriptar(&Gz);
}

//conversion
void Encriptar(int *valor)
{
    valor_normalizado = (*valor / 16) + 2048; 
    mensaje[posicion++] = encriptar[(int)((valor_normalizado & 0x0FC0)>>6)];
    valor_maximo = (int)(valor_normalizado & 0x003F);
    if(valor_maximo == 63)
    {
        valor_maximo = 62;
    }
    mensaje[posicion++] = encriptar[valor_maximo]; 
}

//Bluetooth
void BT_Inicializar(void)
{
    PORTCbits.RC7 = 0;
    PORTCbits.RC6 = 0;
    TRISC7 = 1; //RX
    TRISC6 = 1; //TX
    SPBRG = ( (frecuencia / 16) / ( BT_BoudRate) ) - 1;
    
    //RCSTA
    SPEN = 1;
    RX9 = 0;
    CREN = 1;
    ADDEN = 0;
    
    //TXSTA
    TX9 = 0;
    TXEN = 1;
    SYNC = 0;
    BRGH = 1;
    
    GIE = 0;
    PEIE = 0;
    RCIE = 0;
    TXIE = 0;
}

void BT_Cargar_Caracter(char caracter)
{
    TXREG = caracter;
    while(!TXIF);
    while(!TRMT);
}

void BT_Cargar_Cadena_Texto(char *cadena)
{
    for(int pos = 0; pos < 15; pos++)
    {
        BT_Cargar_Caracter(*cadena++);
    }
    return;
}

void BT_Transmision(char fin_mensaje)
{
    TXREG = fin_mensaje;
    Pausa(1);
}

void BT_Referencia(char ref1, char ref2)
{
    mensaje[13] = ref1;
    mensaje[14] = ref2;
}

unsigned  char BT_Leer_Caracter(void)
{
    if(OERR)
    { 
        CREN = 0; 
        CREN = 1;
    }
    if(RCIF==1)
    {
        while(!RCIF); 
        return RCREG;
    }
    else 
    {
        return 0;
    }   
}
