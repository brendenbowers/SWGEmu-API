using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SWGEmuAPI.Models.Inventory;
using SWGEmuAPI.Models.Inventory.Armor;
using SWGEmuAPI.Models.Inventory.Crafting;
using SWGEmuAPI.Models.Inventory.FactoryCrate;
using SWGEmuAPI.Models.Inventory.Pharmaceutical;
using SWGEmuAPI.Models.Inventory.Resource;
using SWGEmuAPI.Models.Inventory.Weapon;

namespace SWGEmuAPI.Models.Inventory
{
    public static class CharacterInventoryItemExtensions
    {

        public static Model.StringDetailsModel StringDetailsModel { get; set; }
        
        public static Models.Inventory.CharacterInventoryItem ToInventoryItem(this swgemurpcserver.rpc.CharacterInventoryItem inventoryItem)
        {
            Models.Inventory.CharacterInventoryItem returnVal = null;

            if (inventoryItem.HasArmorDetails)
            {
                returnVal = new Models.Inventory.Armor.ArmorInventoryItem()
                {
                    acid = inventoryItem.ArmorDetails.Acid,
                    action_encumberance = inventoryItem.ArmorDetails.ActionEncumberance,
                    blast = inventoryItem.ArmorDetails.Blast,
                    cold = inventoryItem.ArmorDetails.Cold,
                    electricity = inventoryItem.ArmorDetails.Electricity,
                    energy = inventoryItem.ArmorDetails.Energy,
                    health_encumberance = inventoryItem.ArmorDetails.HealthEncumberance,
                    heat = inventoryItem.ArmorDetails.Heat,
                    kinetic = inventoryItem.ArmorDetails.Kinetic,
                    lighsaber = inventoryItem.ArmorDetails.Lighsaber,
                    mind_encumberance = inventoryItem.ArmorDetails.MindEncumberance,
                    rating = (ArmorRating)inventoryItem.ArmorDetails.Rating,
                    stun = inventoryItem.ArmorDetails.Stun,
                    hit_locations = inventoryItem.ArmorDetails.HitLocationList.ToList(),
                };
            }
            else if (inventoryItem.HasWeaponDetails)
            {
                returnVal = new Models.Inventory.Weapon.WeaponInventoryItem()
                {
                     action_attack_cost = inventoryItem.WeaponDetails.ActionAttackCost,
                     armor_piercing = inventoryItem.WeaponDetails.ArmorPiercing,
                     attack_speed = inventoryItem.WeaponDetails.AttackSpeed,
                     damage_radius = inventoryItem.WeaponDetails.DamageRadius,
                     damage_type = (WeaponDamageType)inventoryItem.WeaponDetails.DamageType,
                     force_attack_cost = inventoryItem.WeaponDetails.ForceAttackCost,
                     health_attack_cost = inventoryItem.WeaponDetails.HealthAttackCost,
                     ideal_accuracy = inventoryItem.WeaponDetails.IdealAccuracy,
                     ideal_range = inventoryItem.WeaponDetails.IdealRange,
                     max_damage = inventoryItem.WeaponDetails.MaxDamage,
                     max_range = inventoryItem.WeaponDetails.MaxRange,
                     max_range_accuracy = inventoryItem.WeaponDetails.MaxRangeAccuracy,
                     min_damage = inventoryItem.WeaponDetails.MinDamage,
                     mind_attack_cost = inventoryItem.WeaponDetails.MindAttackCost,
                     point_blank_accuracy = inventoryItem.WeaponDetails.PointBlankAccuracy,
                     point_blank_range = inventoryItem.WeaponDetails.PointBlankRange,
                     weapon_type = inventoryItem.WeaponDetails.WeaponType,
                     wound_ratio = inventoryItem.WeaponDetails.WoundRatio
                };
            }
            else if (inventoryItem.HasResourceDetails)
            {
                returnVal = new Models.Inventory.Resource.ResourceContainerInventoryItem()
                {
                    attributes = inventoryItem.ResourceDetails.AttributesList
                        .ToList()
                        .ConvertAll<ResourceAttribute>(cur => new ResourceAttribute() { name = cur.Name, value = cur.Value })
                        .ToList(),
                    classes = inventoryItem.ResourceDetails.ClassesList
                        .ToList()
                        .ConvertAll<ResourceSpawnClass>(cur => new ResourceSpawnClass() { id = cur.StfClass, name = cur.ClassName })
                        .ToList(),
                    count = inventoryItem.ResourceDetails.Count,
                    resource_id = inventoryItem.ResourceDetails.ResourceId,
                    name = inventoryItem.ResourceDetails.Name,
                    type = inventoryItem.ResourceDetails.Type
                };
            }
            else if (inventoryItem.HasFactoryCrateDetails)
            {
                returnVal = new FactoryCrateInventoryItem()
                {
                     contained_items = inventoryItem.FactoryCrateDetails.ContainedItems.ToInventoryItem(),
                     count = inventoryItem.FactoryCrateDetails.Count,
                     max_items = inventoryItem.FactoryCrateDetails.MaxItems
                };
            }
            else if (inventoryItem.HasPharmaceuticalDetails)
            {
                PharmaceuticalInventoryItem pharmaItem = null;
                switch (inventoryItem.PharmaceuticalDetails.PharmaType)
                {
                    case swgemurpcserver.rpc.PharmaceuticalItem.Types.PharmaceuticalItemType.CUREPACK:
                        pharmaItem = new PharmaceuticalInventoryItem();
                        pharmaItem.pharma_type = PharmaceuticalType.CUREPACK;
                        break;
                    case swgemurpcserver.rpc.PharmaceuticalItem.Types.PharmaceuticalItemType.DOTPACK:
                        pharmaItem = new DOTPackInventorytem()
                        {
                            potency = inventoryItem.PharmaceuticalDetails.DotPackDetails.Potency,
                            duration = inventoryItem.PharmaceuticalDetails.DotPackDetails.Duration,
                            pool = inventoryItem.PharmaceuticalDetails.DotPackDetails.Pool,
                            dot_type = inventoryItem.PharmaceuticalDetails.DotPackDetails.DotType,
                            poison_unit = inventoryItem.PharmaceuticalDetails.DotPackDetails.PoisonUnit,
                            disease_unit = inventoryItem.PharmaceuticalDetails.DotPackDetails.DiseaseUnit
                        };
                        pharmaItem.pharma_type = PharmaceuticalType.DOTPACK;
                        break;
                    case swgemurpcserver.rpc.PharmaceuticalItem.Types.PharmaceuticalItemType.ENHANCEPACK:
                        pharmaItem = new EnhancePackInventoryItem()
                        {
                            duration = inventoryItem.PharmaceuticalDetails.EnhancePackDetails.Duration,
                            attribute = inventoryItem.PharmaceuticalDetails.EnhancePackDetails.Attribute
                        };
                        pharmaItem.pharma_type = PharmaceuticalType.ENHANCEPACK;
                        break;
                    case swgemurpcserver.rpc.PharmaceuticalItem.Types.PharmaceuticalItemType.RANGEDSTIMPACK:
                        pharmaItem = new RnagedStimPackInventoryItem() { range_mod = inventoryItem.PharmaceuticalDetails.RangedStimDetails.RangeMod };
                        pharmaItem.pharma_type = PharmaceuticalType.RANGEDSTIMPACK;
                        break;
                    case swgemurpcserver.rpc.PharmaceuticalItem.Types.PharmaceuticalItemType.REVIVIEPACK:
                        pharmaItem = new RevivePackInventoryItem()
                        {
                            health_wound_healed = inventoryItem.PharmaceuticalDetails.ReviviePackDetails.HealthWoundHealed,
                            health_healed = inventoryItem.PharmaceuticalDetails.ReviviePackDetails.HealthHealed,
                            action_wound_healed = inventoryItem.PharmaceuticalDetails.ReviviePackDetails.ActionWoundHealed,
                            action_healed = inventoryItem.PharmaceuticalDetails.ReviviePackDetails.ActionHealed,
                            mind_wound_healed = inventoryItem.PharmaceuticalDetails.ReviviePackDetails.MindWoundHealed,
                            mind_healed = inventoryItem.PharmaceuticalDetails.ReviviePackDetails.MindHealed
                        };
                        pharmaItem.pharma_type = PharmaceuticalType.REVIVIEPACK;
                        break;
                    case swgemurpcserver.rpc.PharmaceuticalItem.Types.PharmaceuticalItemType.STIMPACK:
                        pharmaItem = new PharmaceuticalInventoryItem();
                        pharmaItem.pharma_type = PharmaceuticalType.STIMPACK;
                        break;
                    case swgemurpcserver.rpc.PharmaceuticalItem.Types.PharmaceuticalItemType.WOUNDPACK:
                        pharmaItem = new WoundPackInventoryItem() { attribute = inventoryItem.PharmaceuticalDetails.WoundPackDetails.Attribute };
                        pharmaItem.pharma_type = PharmaceuticalType.WOUNDPACK;
                        break;
                }

                if (pharmaItem == null)
                {
                    pharmaItem = new PharmaceuticalInventoryItem();
                }

                pharmaItem.area = inventoryItem.PharmaceuticalDetails.Area;
                pharmaItem.effectiveness = inventoryItem.PharmaceuticalDetails.Effectiveness;
                pharmaItem.medicine_required = inventoryItem.PharmaceuticalDetails.MedicineRequired;
                pharmaItem.use_count = inventoryItem.PharmaceuticalDetails.UseCount;
                

                returnVal = pharmaItem;
            }
            else if (inventoryItem.HasCraftingComponentDetails)
            {
                returnVal = new CraftingComponentInventoryItem()
                {
                    attributes = inventoryItem.CraftingComponentDetails.AttributesList
                       .ToList()
                       .ConvertAll<CraftingComponentAttribute>(cur => new CraftingComponentAttribute() 
                        {
                             hidden = cur.Hidden,
                             id = cur.Id,
                             precision = cur.Precision,
                             tite = cur.Tite,
                             value = cur.Value,
                        })
                       .ToList()
                };
            }
            else if (inventoryItem.HasMfgSchemDetails)
            {
                returnVal = new ManufacturingSchematicInventoryItem()
                {
                    manufacture_limit = inventoryItem.MfgSchemDetails.ManufactureLimit,
                    prototype_details = inventoryItem.MfgSchemDetails.PrototypeDetails.ToInventoryItem(),
                    blueprint_entries = inventoryItem.MfgSchemDetails.BlueprintEntriesList
                        .ToList()
                        .ConvertAll<BlueprintEntryItem>(cur => new BlueprintEntryItem()
                        {
                            display_name = cur.DisplayName,
                            identical = cur.Identical,
                            key = cur.Key,
                            quantity = cur.Quantity,
                            serial = cur.Serial,
                            type = cur.Type
                        }).ToList(),
                    ingredients = inventoryItem.MfgSchemDetails.IngredientsList.ToList()
                        .ConvertAll<IngredientItem>(cur => new IngredientItem() 
                        { 
                             identical = cur.Identical,
                             ingredient_slot_name = cur.IngredientSlotName,
                             required_quantity = cur.RequiredQuantity,
                             ingredient = cur.Ingredient.ToInventoryItem()
                        }).ToList()

                };
            }

            if (returnVal == null)
            {
                returnVal = new Models.Inventory.CharacterInventoryItem();
            }

            returnVal.portals_file_name = inventoryItem.PortalsFileName;
            returnVal.template_file_name = inventoryItem.TemplateFileName;
            returnVal.appearance_file_name = inventoryItem.AppearanceFileName;
            returnVal.crafter_name = inventoryItem.CrafterName;
            returnVal.display_name = inventoryItem.DisplayName;
            returnVal.object_id = inventoryItem.ObjectId;
            returnVal.object_name = inventoryItem.ObjectName;
            returnVal.serial_number = inventoryItem.SerialNumber;

            returnVal.condition = inventoryItem.Condition;
            returnVal.max_condition = inventoryItem.MaxCondition;
            

            if (StringDetailsModel != null)
            {
                try
                {
                    returnVal.description = StringDetailsModel.Get(inventoryItem.Description);
                }
                catch(System.IO.FileNotFoundException)
                {
                    
                }
                
            }
            else
            {
                returnVal.description = inventoryItem.Description;
            }
            
            return returnVal;
        }
    }
}