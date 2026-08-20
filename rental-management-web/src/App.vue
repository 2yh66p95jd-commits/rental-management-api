<template>
  <div class="app">
    <aside class="sidebar">
      <div class="brand">
        <h2>RentFlow</h2>
        <span>Management</span>
      </div>

      <nav>
        <RouterLink to="/">Dashboard</RouterLink>
        <RouterLink to="/properties">Properties</RouterLink>
        <RouterLink to="/tenants">Tenants</RouterLink>
        <RouterLink to="/leases">Leases</RouterLink>
        <RouterLink to="/payments">Payments</RouterLink>
      </nav>
    </aside>

    <main class="content">
      <RouterView />
    </main>
  </div>
</template>

<style>
* {
  box-sizing: border-box;
}

body {
  margin: 0;
  font-family: Inter, system-ui, sans-serif;
  background: #f5f7fa;
  color: #17202a;
}

.app {
  min-height: 100vh;
  display: flex;
}

.sidebar {
  width: 240px;
  min-height: 100vh;
  background: #17202a;
  color: white;
  padding: 28px 18px;
}

.brand {
  padding: 0 12px 32px;
}

.brand h2 {
  margin: 0;
}

.brand span {
  font-size: 13px;
  opacity: 0.65;
}

nav {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

nav a {
  color: #cbd5e1;
  text-decoration: none;
  padding: 12px;
  border-radius: 8px;
}

nav a:hover,
nav a.router-link-active {
  background: #2c3e50;
  color: white;
}

.content {
  flex: 1;
  padding: 40px;
}
</style>
