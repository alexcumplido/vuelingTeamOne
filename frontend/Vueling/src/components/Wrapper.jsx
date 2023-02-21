export default function Wrapper({ children }) {
  return (
    <>
      <nav>
        <h1>Vueling</h1>
      </nav>
      <div className="appContainer">{children}</div>
    </>
  );
}
