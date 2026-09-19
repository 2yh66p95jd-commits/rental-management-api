<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { formatCurrency, formatDate } from '../utils/format'

interface Payment {
  id: number
  leaseId: number
  amount: number
  paymentDate: string
  status: string
}

const payments = ref<Payment[]>([])
const loading = ref(true)
const error = ref('')

onMounted(async () => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/api/Payments`
    )

    if (!response.ok) {
      throw new Error('Failed to load payments')
    }

    payments.value = await response.json()
  } catch (err) {
    console.error(err)
    error.value = 'Unable to load payments.'
  } finally {
    loading.value = false
  }
})

const statusClass = (status: string) =>
  status.toLowerCase() === 'paid' ? 'badge-paid' : 'badge-pending'
</script>

<template>
  <section class="payments">
    <header class="page-header">
      <p class="eyebrow">RENTAL MANAGEMENT</p>
      <h1>Payments</h1>
      <p class="subtitle">Rent payments received against leases</p>
    </header>

    <div v-if="loading" class="state">Loading payments...</div>

    <div v-else-if="error" class="state error">{{ error }}</div>

    <div v-else-if="payments.length === 0" class="state">
      No payments yet.
    </div>

    <div v-else class="card">
      <table>
        <thead>
          <tr>
            <th>Lease</th>
            <th>Amount</th>
            <th>Date</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="payment in payments" :key="payment.id">
            <td class="name">#{{ payment.leaseId }}</td>
            <td>{{ formatCurrency(payment.amount) }}</td>
            <td>{{ formatDate(payment.paymentDate) }}</td>
            <td>
              <span class="badge" :class="statusClass(payment.status)">
                {{ payment.status }}
              </span>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </section>
</template>

<style scoped>
.payments {
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

.badge-paid {
  background: #e3f6e8;
  color: #15803d;
}

.badge-pending {
  background: #fdf1d6;
  color: #a15c07;
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
