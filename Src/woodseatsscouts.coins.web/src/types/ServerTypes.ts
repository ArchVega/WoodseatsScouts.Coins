export type Member = {
  firstName: string
  lastName: string
  hasImage: boolean
  memberScoutGroupNumber: number
  memberSection: string
  memberId: number
  memberCode: string
  memberNumber: number
  memberSectionName: string
  memberScoutGroupName: string
}

export type MembersWithPoints = {
  id: number
  firstName: string
  lastName: string
  fullName: string
  hasImage: boolean
  memberCode: string
  memberNumber: number
  scoutGroupName: string
  sectionId: string
  sectionName: string
  totalPoints: number
}