public class WalkerBehavior {

    //private TerrainWalker walker
    private IWalker walker;

    public void Init() {
        walker = new TerrainWalker(terrain);
        /* In future:
        walker = levelGeometryFactory.createWalker();
        */
    }

    void Update() {
        walker.rotate(angle);
        walker.walk(direction, distance);

        transform.position = walker.Position;
        transform.rotation = walker.Rotation;
    }
}
