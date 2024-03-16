public class CannonTower : Tower
{
    protected override void CreateProjectile()
    {
        _projectilesSpawner.GetProjectile();
    }
}

