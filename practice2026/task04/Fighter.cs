namespace Task04;

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
