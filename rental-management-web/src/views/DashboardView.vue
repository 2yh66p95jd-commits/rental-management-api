<script setup lang="ts">
import { onMounted, ref } from 'vue'

interface DashboardSummary {
  totalProperties: number
  occupiedProperties: number
  availableProperties: number
  occupancyRate: number
  activeLeases: number
  monthlyRentalIncome: number
  paymentsThisMonth: number
  outstandingPayments: number
}

const dashboard = ref<DashboardSummary | null>(null)
const loading = ref(true)
const error = ref('')

onMounted(async () => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/api/Dashboard/summary`
    )

    if (!response.ok) {
      throw new Error('Failed to load dashboard')
    }

    dashboard.value = await response.json()
  } catch (err) {
    console.error(err)
    error.value = 'Unable to load dashboard data.'
  } finally {
    loading.value = false
  }
})

const formatCurrency = (value: number) =>
  new Intl.NumberFormat('en-ZA', {
    style: 'currency',
    currency: 'ZAR'
  }).format(value)
</script>

<template>
  <section class="dashboard">
    <header class="page-header">
      <p class="eyebrow">RENTAL MANAGEMENT</p>
      <h1>Dashboard</h1>
      <p class="subtitle">Overview of your rental portfolio</p>
    </header>

    <div v-if="loading" class="state">
      Loading dashboard...
    </div>

    <div v-else-if="error" class="state error">
      {{ error }}
    </div>

    <div v-else-if="dashboard" class="stats-grid">

      <article class="stat-card">
        <span class="label">Total Properties</span>
        <strong>{{ dashboard.totalProperties }}</strong>
      </article>

      <article class="stat-card">
        <span class="label">Occupancy Rate</span>
        <strong>{{ dashboard.occupancyRate }}%</strong>
        <span class="detail">
          {{ dashboard.occupiedProperties }} occupied ·
          {{ dashboard.availableProperties }} available
        </span>
      </article>

      <article class="stat-card">
        <span class="label">Monthly Rental Income</span>
        <strong>{{ formatCurrency(dashboard.monthlyRentalIncome) }}</strong>
        <span class="detail">
          {{ dashboard.activeLeases }} active leases
        </span>
      </article>

      <article class="stat-card">
        <span class="label">Payments This Month</span>
        <strong>{{ formatCurrency(dashboard.paymentsThisMonth) }}</strong>
      </article>

      <article class="stat-card">
        <span class="label">Outstanding Payments</span>
        <strong>{{ formatCurrency(dashboard.outstandingPayments) }}</strong>
      </article>

    </div>
  </section>
</template>

<style scoped>
.dashboard {
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

.stats-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 22px;
}

.stat-card {
  min-height: 170px;
  padding: 28px;
  background: white;
  border-radius: 18px;
  box-shadow: 0 4px 18px rgba(0, 0, 0, 0.06);
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.stat-card:first-child {
  grid-column: span 2;
}

.label {
  margin-bottom: 18px;
  font-size: 17px;
  font-weight: 600;
  color: #68737d;
}

.stat-card strong {
  font-size: 38px;
  line-height: 1.1;
  color: #17202a;
}

.detail {
  margin-top: 12px;
  color: #68737d;
  font-size: 15px;
}

.state {
  padding: 28px;
  background: white;
  border-radius: 18px;
}

.error {
  color: #b42318;
}

@media (max-width: 700px) {
  .stats-grid {
    grid-template-columns: 1fr;
  }

  .stat-card:first-child {
    grid-column: span 1;
  }

  h1 {
    font-size: 36px;
  }
}
</style>
