export type MemberDto = {
  id: number;
  code: string
  number: number
  firstName: string
  lastName: string
  fullName: string
  scoutGroupId: number
  scoutGroupName: string
  sectionId: string
  sectionName: string
  clue1State: string
  clue2State: string
  clue3State: string
  isDayVisitor: boolean
  hasImage: boolean
  imagePath: string
  clientComputedImageUri: string
}

export type CoinDto = {
  code: string
  baseNumber: number
  pointValue: number
}

// old ---------------------------------------------------------------------------v
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

export type HaulResult = {
  scavengerResultId: number
  hauledAtIso8601: string
  totalPoints: number
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
  haulResult: HaulResult[]
  latestHaulResult?: HaulResult | undefined
  selectedHaulResultId: number | undefined
  selectedHaulResult?: HaulResult | undefined
}