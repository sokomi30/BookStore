import { Navigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import type { ReactNode } from 'react'

interface Props {
  children: ReactNode
  adminOnly?: boolean
}

function ProtectedRoute({ children, adminOnly = false }: Props) {
  const { user } = useAuth()

  if (!user) return <Navigate to="/login" />

  if (adminOnly && user.role !== 'Admin') return <Navigate to="/" />

  return <>{children}</>
}

export default ProtectedRoute