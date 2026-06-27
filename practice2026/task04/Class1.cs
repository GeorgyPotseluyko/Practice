using System;

public interface ISpaceship
{
    void MoveForward();      // Движение вперед
    void Rotate(int angle);  // Поворот на угол (градусы)
    void Fire();             // Выстрел ракетой
    int Speed { get; }       // Скорость корабля
    int FirePower { get; }   // Мощность выстрела
}

public class Cruiser : ISpaceship
{
    public int Speed { get; }
    public int FirePower { get; } 

    public Cruiser(int speed = 50, int firepower = 100)
    {
        Speed = speed;
        FirePower = firepower;
    }

    public void MoveForward(){}
    public void Rotate(int angle){}
    public void Fire(){}
}

public class Fighter : ISpaceship
{
    public int Speed { get; }
    public int FirePower { get; }

    public Fighter(int speed = 100, int firepower = 50)
    {
        Speed = speed;
        FirePower = firepower;
    }

    public void MoveForward(){}
    public void Rotate(int angle){}
    public void Fire(){}
}