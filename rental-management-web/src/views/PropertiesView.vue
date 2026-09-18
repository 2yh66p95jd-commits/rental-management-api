<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { formatCurrency } from '../utils/format'

interface Property {
  id: number
  address: string
  city: string
  propertyType: string
  bedrooms: number
  monthlyRent: number
  isAvailable: boolean
}

const properties = ref<Property[]>([])
const loading = ref(true)
const error = ref('')

onMounted(async () => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/api/Properties`
    )

    if (!response.ok) {
      throw new Error('Failed to load properties')
    }

    properties.value = await response.json()
  } catch (err) {
    console.error(err)
    error.value = 'Unable to load properties.'
  } finally {
    loading.value = false
  }
})
</script>

<template>
  <section class="properties">
    <header class="page-header">
      <p class="eyebrow">RENTAL MANAGEMENT</p>
      <h1>Properties</h1>
      <p class="subtitle">Manage your rental properties</p>
    </header>

    <div v-if="loading" class="state">Loading properties...</div>

    <div v-else-if="error" class="state error">{{ error }}</div>

    <div v-else-if="properties.length === 0" class="state">
      No properties yet.
    </div>

    <div v-else class="properties-grid">
      <article
        v-for="property in properties"
        :key="property.id"
        class="property-card"
      >
        <div class="card-top">
          <h2>{{ property.address }}</h2>
          <span
            class="badge"
            :class="property.isAvailable ? 'badge-available' : 'badge-occupied'"
          >
            {{ property.isAvailable ? 'Available' : 'Occupied' }}
          </span>
        </div>

        <p class="city">{{ property.city }}</p>

        <div class="meta">
          <span>{{ property.propertyType }}</span>
          <span>{{ property.bedrooms }} bed{{ property.bedrooms === 1 ? '' : 's' }}</span>
        </div>

        <strong class="rent">{{ formatCurrency(property.monthlyRent) }} / month</strong>
      </article>
    </div>
  </section>
</template>

<style scoped>
.properties {
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

.properties-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 22px;
}

.property-card {
  padding: 26px;
  background: white;
  border-radius: 18px;
  box-shadow: 0 4px 18px rgba(0, 0, 0, 0.06);
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.card-top {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 12px;
}

.card-top h2 {
  margin: 0;
  font-size: 20px;
  line-height: 1.3;
}

.badge {
  flex-shrink: 0;
  padding: 4px 10px;
  border-radius: 999px;
  font-size: 12px;
  font-weight: 700;
  white-space: nowrap;
}

.badge-available {
  background: #e3f6e8;
  color: #15803d;
}

.badge-occupied {
  background: #eef0f2;
  color: #68737d;
}

.city {
  margin: 0;
  color: #68737d;
}

.meta {
  display: flex;
  gap: 14px;
  color: #68737d;
  font-size: 14px;
}

.rent {
  margin-top: 8px;
  font-size: 20px;
  color: #17202a;
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
