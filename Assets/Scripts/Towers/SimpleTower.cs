public class SimpleTower : Tower
{
    protected override void CreateProjectile()
    {
        (_projectilesSpawner.GetProjectile() as GuidedProjectile).SetTarget(_nearestMonster.transform);
    }
}
