public class CachedMember<T> {
    public T Value {
        get {
            if (value == null) {
                value = initializer();
            }
            return value;
        }
    }
    private T value;

    public delegate T Initializer();
    private Initializer initializer;

    public CachedMember(Initializer initializer) {
        this.initializer = initializer;
    }
}