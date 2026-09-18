<script setup lang="ts">
import { onMounted, ref } from 'vue'

interface Tenant {
  id: number
  fullName: string
  email: string
  phoneNumber: string
}

const tenants = ref<Tenant[]>([])
const loading = ref(true)
const error = ref('')

onMounted(async () => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/api/Tenants`
    )

    if (!response.ok) {
      throw new Error('Failed to load tenants')
    }

    tenants.value = await response.json()
  } catch (err) {
    console.error(err)
    error.value = 'Unable to load tenants.'
  } finally {
    loading.value = false
  }
})
</script>

<template>
  <section class="tenants">
    <header class="page-header">
      <p class="eyebrow">RENTAL MANAGEMENT</p>
      <h1>Tenants</h1>
      <p class="subtitle">People renting your properties</p>
    </header>

    <div v-if="loading" class="state">Loading tenants...</div>

    <div v-else-if="error" class="state error">{{ error }}</div>

    <div v-else-if="tenants.length === 0" class="state">No tenants yet.</div>

    <div v-else class="card">
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Phone</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="tenant in tenants" :key="tenant.id">
            <td class="name">{{ tenant.fullName }}</td>
            <td>{{ tenant.email }}</td>
            <td>{{ tenant.phoneNumber }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </section>
</template>

<style scoped>
.tenants {
  max-width: 1100px;
}

.page-header {
  margin-bottom: 36px;
}

.eyebrow {
  margin: 0 0 10px;
  font-size: 13px;
  font-weight: 800;
  letter-spacing: 0.14em;
  color: #68737d;
}

h1 {
  margin: 0;
  font-size: 42px;
  line-height: 1.1;
}

.subtitle {
  margin: 12px 0 0;
  font-size: 18px;
  color: #68737d;
}

.card {
  background: white;
  border-radius: 18px;
  box-shadow: 0 4px 18px rgba(0, 0, 0, 0.06);
  overflow: hidden;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  text-align: left;
  padding: 16px 24px;
}

th {
  font-size: 13px;
  font-weight: 700;
  letter-spacing: 0.04em;
  color: #68737d;
  text-transform: uppercase;
  border-bottom: 1px solid #eef0f2;
}

td {
  border-bottom: 1px solid #eef0f2;
  color: #17202a;
}

tr:last-child td {
  border-bottom: none;
}

.name {
  font-weight: 600;
}

.state {
  padding: 28px;
  background: white;
  border-radius: 18px;
}

.error {
  color: #b42318;
}
</style>
