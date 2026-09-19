<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { formatCurrency, formatDate } from '../utils/format'

interface Lease {
  id: number
  propertyId: number
  tenantId: number
  propertyAddress: string
  tenantName: string
  startDate: string
  endDate: string
  monthlyRent: number
  status: string
}

const leases = ref<Lease[]>([])
const loading = ref(true)
const error = ref('')

onMounted(async () => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/api/Leases`
    )

    if (!response.ok) {
      throw new Error('Failed to load leases')
    }

    leases.value = await response.json()
  } catch (err) {
    console.error(err)
    error.value = 'Unable to load leases.'
  } finally {
    loading.value = false
  }
})

const statusClass = (status: string) =>
  status.toLowerCase() === 'active' ? 'badge-active' : 'badge-inactive'
</script>

<template>
  <section class="leases">
    <header class="page-header">
      <p class="eyebrow">RENTAL MANAGEMENT</p>
      <h1>Leases</h1>
      <p class="subtitle">Active and past lease agreements</p>
    </header>

    <div v-if="loading" class="state">Loading leases...</div>

    <div v-else-if="error" class="state error">{{ error }}</div>

    <div v-else-if="leases.length === 0" class="state">No leases yet.</div>

    <div v-else class="card">
      <table>
        <thead>
          <tr>
            <th>Property</th>
            <th>Tenant</th>
            <th>Start</th>
            <th>End</th>
            <th>Monthly Rent</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="lease in leases" :key="lease.id">
            <td class="name">{{ lease.propertyAddress }}</td>
            <td>{{ lease.tenantName }}</td>
            <td>{{ formatDate(lease.startDate) }}</td>
            <td>{{ formatDate(lease.endDate) }}</td>
            <td>{{ formatCurrency(lease.monthlyRent) }}</td>
            <td>
              <span class="badge" :class="statusClass(lease.status)">
                {{ lease.status }}
              </span>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </section>
</template>

<style scoped>
.leases {
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
  overflow: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  text-align: left;
  padding: 16px 24px;
  white-space: nowrap;
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

.badge {
  padding: 4px 10px;
  border-radius: 999px;
  font-size: 12px;
  font-weight: 700;
}

.badge-active {
  background: #e3f6e8;
  color: #15803d;
}

.badge-inactive {
  background: #eef0f2;
  color: #68737d;
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
