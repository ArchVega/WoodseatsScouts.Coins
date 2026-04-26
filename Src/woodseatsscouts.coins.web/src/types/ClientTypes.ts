export type SectionBranding = {
  foregroundColour: string;
  backgroundColour: string
}

export type AdditionalData = {
  hasAnomalyOccurred: boolean
  anomalousCoinsTotalValue: boolean
}

export type HaulResult = {
  coinTotal: number,
  additionalData: AdditionalData
}

export type UpdateScoutMemberRequestPayload = {
  firstName: string,
  lastName: string,
  scoutGroupId: number,
  scoutSectionCode: string,
}

export type UnscavengedCoin = {
  code: string
  value: number
  fullName: string,
  isAlreadyScavenged: boolean
}