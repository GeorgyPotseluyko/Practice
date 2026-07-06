namespace Task04;

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
