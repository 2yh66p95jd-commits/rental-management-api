<script setup lang="ts">
import { onMounted, ref } from 'vue'

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
  <section>
    <h1>Properties</h1>
    <p>Manage your rental properties.</p>

    <p v-if="loading">Loading properties...</p>

    <p v-else-if="error">{{ error }}</p>

    <div v-else>
      <article
        v-for="property in properties"
        :key="property.id"
      >
        <h2>{{ property.address }}</h2>
        <p>{{ property.city }}</p>
        <p>{{ property.propertyType }}</p>
        <p>{{ property.bedrooms }} bedrooms</p>
        <p>R{{ property.monthlyRent }}</p>

        <strong>
          {{ property.isAvailable ? 'Available' : 'Occupied' }}
        </strong>
      </article>
    </div>
  </section>
</template>
